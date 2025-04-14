
// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta
// (Clarified meaning of dependent and dependee.)
// (Clarified names in solution/project structure.)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SpreadsheetUtilities
{
    /// <summary>
    /// Author:    Bingying Wang
    /// Partner:   None
    /// Date:      01/25/2024
    /// Course:    CS 3500, University of Utah, School of Computing
    /// Copyright: CS 3500 and Bingying - This work may not 
    ///            be copied for use in Academic Coursework.
    ///
    /// I, Bingying Wang, certify that I wrote this code from scratch and
    /// did not copy it in part or whole from another source.  All 
    /// references used in the completion of the assignments are cited 
    /// in my README file.
    ///
    /// File Contents
    ///
    /// The project contains methods that construct a DependencyGraph.
    /// A graph represents nodes and edges between the nodes. And we
    /// could use this to work with variables.
    /// In DependencyGraph we have ordered pair of strings.
    /// 
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    ///
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings. Two
    /// ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1
    ///equals t2.
    /// Recall that sets never contain duplicates. If an attempt is made to add an
    ///element to a
    /// set, and the element is already in the set, the set remains unchanged.
    ///
    /// Given a DependencyGraph DG:
    ///
    /// (1) If s is a string, the set of all strings t such that (s,t) is in DG is
    ///called dependents(s).
    /// (The set of things that depend on s)
    ///
    /// (2) If s is a string, the set of all strings t such that (t,s) is in DG is
    ///called dependees(s).
    /// (The set of things that s depends on)
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    // dependents("a") = {"b", "c"}
    // dependents("b") = {"d"}
    // dependents("c") = {}
    // dependents("d") = {"d"}
    // dependees("a") = {}
    // dependees("b") = {"a"}
    // dependees("c") = {"a"}
    // dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        //HashSet avoid duplicates
        //HashSet contains all the items depond on it or it depends on
        private Dictionary<string, HashSet<string>> dependents;
        private Dictionary<string, HashSet<string>> dependees;
        //pairCount keeps track of the amount of pairs in graph
        private int pairCount;
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            //initialize three instance variables
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
            pairCount = 0;
        }
        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return pairCount; }//pairCount keeps track of size
        }
        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer. If dg is a DependencyGraph, you
        ////would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (dependees.ContainsKey(s))//if the key exists
                    return dependees[s].Count;//we directly return the count of dependees[s] 
                else return 0;//otherwise, return 0
            }
        }
        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            //we need to check if the key exists in dependents first
            if (dependents.ContainsKey(s))
            {
                if (dependents[s].Count != 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            //we need to check if the key exists in dependees first
            if (dependees.ContainsKey(s))
            {
                if (dependees[s].Count != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            //create an empty List<string>, and this list is what we want to return
            List<string> deps = new List<string>();

            if (dependents.ContainsKey(s))
            {
                //loop through each value-HashSet in dependents
                foreach (var dep in dependents[s])
                {
                    deps.Add(dep);//add the value string into list
                }
            }

            return deps;
        }
        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            List<string> deps = new List<string>();

            if (dependees.ContainsKey(s))
            {
                foreach (var dep in dependees[s])
                {
                    deps.Add(dep);
                }
            }

            return deps;
        }
        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        ///
        /// <para>This should be thought of as:</para>
        ///
        /// t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param> ///
        public void AddDependency(string s, string t)
        {
            //if dependents doesn't have the key "s"
            if (!dependents.ContainsKey(s))
            {
                //we need to create a new HashSet since the value type is HashSet<string>
                dependents[s] = new HashSet<string>();
            }

            //if dependees doesn't have the key "t"
            if (!dependees.ContainsKey(t))
            {
                //we need to create a new HashSet since the value type is HashSet<string>
                dependees[t] = new HashSet<string>();
            }
            //The Add method returns true if the element was added to the set
            //(if it was not in the HashSet)
            //and false if the element was already in the HashSet
            if (dependents[s].Add(t))
            {
                pairCount++;//increment pairCount by 1
            }
            dependees[t].Add(s);//adding value into my dependees will not affect my pairCount
        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            //if the dependents does have the key "s"
            if (dependents.ContainsKey(s))
            {
                dependents[s].Remove(t);//we remove the correponding value from dependents
                dependees[t].Remove(s);//we remove the correponding value from dependees
                pairCount--;//decrease pairCount by 1
            }
            ////if the dependees does have the key "t"
            //else if (dependees.ContainsKey(t))
            //{
            //    dependents[s].Remove(t);//we remove the correponding value from dependents
            //    dependees[t].Remove(s);//we remove the correponding value from dependees
            //    pairCount--;//decrease pairCount by 1
            //}
        }
        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r). Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            //get dependents and make it into a list,
            //and store that list into a List<string>
            List<string> dependents = GetDependents(s).ToList();
            foreach (string dependent in dependents)
                RemoveDependency(s, dependent);//invoke RemoveDependency to get rid of remove ordered pairs
            //loop through the new dependents and get its value
            foreach (string dependent2 in newDependents)
                AddDependency(s, dependent2);//invoke AddDependency to add ordered pairs
        }
        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s). Then, for each
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            //get dependees and make it into a list,
            //and store that list into a List<string>
            List<string> dependees = GetDependees(s).ToList();
            foreach (var dependee in dependees)
                RemoveDependency(dependee, s);//invoke RemoveDependency to get rid of remove ordered pairs

            foreach (var dependee2 in newDependees)
                AddDependency(dependee2, s);//invoke AddDependency to add ordered pairs
        }
    }
}

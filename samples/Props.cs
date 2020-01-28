using System;
using System.Collections.Generic;

// Namespace doesn't matter
namespace T4Copy 
{ 
    // Class name doesn't matter - just make class public
    public class Props 
    {
        public Props(IDictionary<string, string> properties, Action<string, object[]> logMessage)
        {
            logMessage("Received {count} properties", new object[]{properties.Count});

            Project = Namespace = properties["name"];
            Solution = Project.Replace('.', '-').ToLower();
            SourcesPath = Project.Replace('.', System.IO.Path.DirectorySeparatorChar);            
        }

        public string Author => Environment.GetEnvironmentVariable("USERNAME") ?? Environment.GetEnvironmentVariable("USER");

        public string Project {get;}

        public string SourcesPath {get;}

        public string Namespace {get;}        

        public string Solution {get;}

        public string Date => DateTime.Now.ToShortDateString();
    }
}
﻿using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace McNeelCourse
{
    public class McNeelCourseInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "McNeelCourse";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("2ed786ee-1785-4a2c-997b-105dfa98e7e7");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}

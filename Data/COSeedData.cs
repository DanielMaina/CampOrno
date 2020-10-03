﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CampOrno.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CampOrno.Data
{
    public static class COSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new CampOrnoContext(
                serviceProvider.GetRequiredService<DbContextOptions<CampOrnoContext>>()))
            {
                //Create sample data with some random values
                Random random = new Random();

                string[] firstNames = new string[] { "Fred", "Barney", "Wilma", "Betty" };
                string[] kidNames = new string[] { "Woodstock", "Sally", "Violet", "Charlie", "Lucy", "Linus", "Franklin", "Marcie", "Schroeder" };
                string[] lastNames = new string[] { "Stovell", "Jones", "Bloggs", "Flintstone", "Rubble", "Brown", "Smith", "Daniel" };
                string[] nicknames = new string[] { "Hightower", "Wizard", "Kingfisher", "Prometheus", "Broomspun", "Shooter", "Chuckles" };
                string[] compounds = new string[] { "Nestlings", "Fledglings", "Sharpies" };
                string[] diets = new string[] { "Peanuts", "Shellfish", "Gluten", "Lactose", "Halal" };
                string[] genders = new string[] { "M", "F", "N", "T", "O" };
                string[] activities = new string[] { "Canoeing", "Archery", "Ice Fishing", "Karate", "Coding" };

                //For seeding pictures
                string[] pictures = new string[] { "Barney.png", "blankProfile.png", "Fred.png", "Pebbles.png", "Wilma.png" };

                //For Part 5
                //Activities
                if (!context.Activities.Any())
                {
                    //loop through the array of Compound names
                    foreach (string a in activities)
                    {
                        Activity act = new Activity()
                        {
                            Name = a
                        };
                        context.Activities.Add(act);
                    }
                    context.SaveChanges();
                }

                //Dietary Restrictions
                if (!context.DietaryRestrictions.Any())
                {
                    //loop through the array of Compound names
                    foreach (string d in diets)
                    {
                        DietaryRestriction diet = new DietaryRestriction()
                        {
                            Name = d
                        };
                        context.DietaryRestrictions.Add(diet);
                    }
                    context.SaveChanges();
                }

                //Compounds
                if (!context.Compounds.Any())
                {
                    //loop through the array of Compound names
                    foreach (string c in compounds)
                    {
                        Compound compound = new Compound()
                        {
                            Name = c
                        };
                        context.Compounds.Add(compound);
                    }
                    context.SaveChanges();
                }
                //Counselors
                if (context.Counselors.Count() == 0)
                {
                    List<Counselor> counselors = new List<Counselor>();
                    foreach (string lastName in lastNames)
                    {
                        foreach (string firstname in firstNames)
                        {
                            //Construct some counselor details
                            Counselor newCounselor = new Counselor()
                            {
                                FirstName = firstname,
                                LastName = lastName,
                                MiddleName = lastName[1].ToString().ToUpper(),
                                SIN = random.Next(213214131, 989898989).ToString(),
                            };
                            counselors.Add(newCounselor);
                        }
                    }
                    //Now give a few of them nicknames. 
                    //We don't want any duplicates so choose one counselor 
                    //for each nickname
                    int numNames = nicknames.Count();
                    int numCounselors = counselors.Count();
                    for (int i = 0; i < numNames; i++)
                    {
                        counselors[random.Next(0, numNames)].Nickname = nicknames[i];
                    }
                    //Now add your list into the DbSet
                    context.Counselors.AddRange(counselors);
                    context.SaveChanges();
                }
                //Create collections of the primary keys
                int[] counselorIDs = context.Counselors.Select(a => a.ID).ToArray();
                int counselorIDCount = counselorIDs.Count();
                int[] compoundIDs = context.Compounds.Select(a => a.ID).ToArray();
                int compoundIDCount = compoundIDs.Count();
                int genderCount = genders.Count();
                int pictureCount = pictures.Count();

                //Campers
                if (!context.Campers.Any())
                {
                    // Start birthdate for randomly produced campers 
                    // We will subtract a random number of days from today
                    DateTime startDOB = DateTime.Today;

                    List<Camper> campers = new List<Camper>();
                    int toggle = 1; //Used to alternate assigning counselors
                    foreach (string lastName in lastNames)
                    {
                        foreach (string kidname in kidNames)
                        {
                            //Construct some employee details
                            Camper newCamper = new Camper()
                            {
                                FirstName = kidname,
                                LastName = lastName,
                                MiddleName = kidname[1].ToString().ToUpper(),
                                DOB = startDOB.AddDays(-random.Next(1480, 5800)),
                                Gender = genders[random.Next(genderCount)],
                                //Uncomment these lines to seed profile pictures
                                //imageContent = imageToBytes("wwwroot/pictures/" + pictures[random.Next(pictureCount)]),
                                //imageMimeType = "image/png",
                                //imageFileName = "Profile.png",
                                eMail = (kidname.Substring(0, 2) + lastName + random.Next(11, 111).ToString() + "@outlook.com").ToLower(),
                                Phone = Convert.ToInt64(random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString()),
                                CompoundID = compoundIDs[random.Next(compoundIDCount)]
                            };
                            if (toggle % 2 == 0)//Every second camper gets a lead counselor assigned
                            {
                                newCamper.CounselorID = counselorIDs[random.Next(counselorIDCount)];
                            }
                            toggle++;
                            campers.Add(newCamper);
                        }
                    }
                    //Now add your list into the DbSet
                    context.Campers.AddRange(campers);
                    context.SaveChanges();
                }

                //Create collections of the primary keys
                int[] camperIDs = context.Campers.Select(a => a.ID).ToArray();
                int camperIDCount = camperIDs.Count();
                int[] dietaryRestrictionIDs = context.DietaryRestrictions.Select(a => a.ID).ToArray();
                int dietaryRestrictionIDCount = dietaryRestrictionIDs.Count();
                //For Part 5
                int[] activityIDs = context.Activities.Select(a => a.ID).ToArray();
                int activityIDCount = activityIDs.Count();

                //For Part 5
                //CamperActivities - the Intersection
                //Add an activity to every second Camper
                if (!context.CamperActivities.Any())
                {
                    int toggle = 1; //Used to alternate
                    for (int i = 0; i < camperIDCount; i++)
                    {
                        if (toggle % 2 == 0)//Every second camper 
                        {

                            CamperActivity ds = new CamperActivity()
                            {
                                CamperID = camperIDs[i],
                                ActivityID = activityIDs[random.Next(activityIDCount)]
                            };
                            context.CamperActivities.Add(ds);
                        }
                        toggle++;
                    }
                    context.SaveChanges();
                }

                //CamperDiets - the Intersection
                //Add a few restrictions to every second Camper
                if (!context.CamperDiets.Any())
                {
                    int toggle = 1; //Used to alternate
                    for (int i = 0; i < camperIDCount; i++)
                    {
                        if (toggle % 2 == 0)//Every second camper 
                        {
                            
                            CamperDiet ds = new CamperDiet()
                            {
                                CamperID = camperIDs[i],
                                DietaryRestrictionID = dietaryRestrictionIDs[random.Next(dietaryRestrictionIDCount)]
                            };
                            context.CamperDiets.Add(ds);
                        }                       
                        toggle++;
                    }
                    context.SaveChanges();
                }
                //CounselorCompounds - the Intersection
                //Add a compound to each counselor
                if (!context.CounselorCompounds.Any())
                {
                    foreach (int i in counselorIDs)
                    {
                        CounselorCompound cc = new CounselorCompound()
                        {
                            CompoundID = compoundIDs[random.Next(compoundIDCount)],
                            CounselorID = i
                        };
                        context.CounselorCompounds.Add(cc);
                    }
                }
                context.SaveChanges();
            }
        }
        public static Byte[] imageToBytes(string sPath)
        {
            //Initialize byte array with a null value initially.
            byte[] data = null;

            //Use FileInfo object to get file size.
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;

            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to supply number of bytes 
            //to read from file.
            //In this case we want to read entire file. 
            //So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);

            return data;
        }
    }
}


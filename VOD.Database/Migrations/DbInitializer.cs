using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VOD.Common.Entities;
using VOD.Database.Contexts;

namespace VOD.Database.Migrations
{
    public class DbInitializer
    {
        public static void RecreateDatabase(VODContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static void Initialize(VODContext context)
        {
            #region Lorem Ipsum - Dummy Data
            var description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            #endregion

            #region Admin Credentials Properties
            /*
                The email address should be in the AspNetUsers table; if not, 
                then register a user with that email address or change the variable 
                value to an email address in the table. The user should be an 
                administrator; if not, open the AspNetUserRoles table and add 
                a record using the user id and 1 (or the id you gave the Admin
                role in the AspNetRoles table) in the RoleId column.
             */
            var email = "a@b.c";
            var adminRoleId = string.Empty;
            var userId = string.Empty;
            #endregion

            // Fetch the User Data
            if (context.Users.Any(r => r.Email.Equals(email)))
                userId = context.Users.First(r => r.Email.Equals(email)).Id;

            if (!userId.Equals(string.Empty))
            {
                #region Add Instructors if they don't already exist
                if (!context.Instructors.Any())
                {
                    var instructors = new List<Instructor>
                    {
                        new Instructor {
                            Name = "John Doe",
                            Description = description.Substring(20, 50),
                            Thumbnail = "/images/Ice-Age-Scrat-icon.png"
                        },
                        new Instructor {
                            Name = "Jane Doe",
                            Description = description.Substring(30, 40),
                            Thumbnail = "/images/Ice-Age-Scrat-icon.png"
                        }
                    };
                    context.Instructors.AddRange(instructors);
                    context.SaveChanges();
                }
                #endregion

                #region Add Courses if they don't already exist
                if (!context.Courses.Any())
                {
                    var instructorId1 = context.Instructors.First().Id;
                    var instructorId2 = int.MinValue;
                    var instructor = context.Instructors.Skip(1).FirstOrDefault();
                    if (instructor != null) instructorId2 = instructor.Id;
                    else instructorId2 = instructorId1;

                    var courses = new List<Course>
                    {
                        new Course {
                            InstructorId = instructorId1,
                            Title = "Course 1",
                            Description = description,
                            ImageUrl = "/images/course1.jpg",
                            MarqueeImageUrl = "/images/laptop.jpg"
                        },
                        new Course {
                            InstructorId = instructorId2,
                            Title = "Course 2",
                            Description = description,
                            ImageUrl = "/images/course2.jpg",
                            MarqueeImageUrl = "/images/laptop.jpg"
                        },
                        new Course {
                            InstructorId = instructorId1,
                            Title = "Course 3",
                            Description = description,
                            ImageUrl = "/images/course3.jpg",
                            MarqueeImageUrl = "/images/laptop.jpg"
                        }
                    };
                    context.Courses.AddRange(courses);
                    context.SaveChanges();
                }
                #endregion

                #region Fetch Course ids if any courses exists
                var courseId1 = int.MinValue;
                var courseId2 = int.MinValue;
                var courseId3 = int.MinValue;
                if (context.Courses.Any())
                {
                    courseId1 = context.Courses.First().Id;

                    var course = context.Courses.Skip(1).FirstOrDefault();
                    if (course != null) courseId2 = course.Id;

                    course = context.Courses.Skip(2).FirstOrDefault();
                    if (course != null) courseId3 = course.Id;
                }
                #endregion

                #region Add UserCourses connections if they don't already exist
                if (!context.UserCourses.Any())
                {
                    if (!courseId1.Equals(int.MinValue))
                        context.UserCourses.Add(new UserCourse { UserId = userId, CourseId = courseId1 });

                    if (!courseId2.Equals(int.MinValue))
                        context.UserCourses.Add(new UserCourse { UserId = userId, CourseId = courseId2 });

                    if (!courseId3.Equals(int.MinValue))
                        context.UserCourses.Add(new UserCourse { UserId = userId, CourseId = courseId3 });

                    context.SaveChanges();
                }
                #endregion

                #region Add Modules if they don't already exist
                if (!context.Modules.Any())
                {
                    var modules = new List<Module>
                    {
                        new Module { Course = context.Find<Course>(courseId1), Title = "Modeule 1" },
                        new Module { Course = context.Find<Course>(courseId1), Title = "Modeule 2" },
                        new Module { Course = context.Find<Course>(courseId2), Title = "Modeule 3" }
                    };
                    context.Modules.AddRange(modules);
                    context.SaveChanges();
                }
                #endregion

                #region Fetch Module ids if any modules exist
                var moduleId1 = int.MinValue;
                var moduleId2 = int.MinValue;
                var moduleId3 = int.MinValue;
                if (context.Modules.Any())
                {
                    moduleId1 = context.Modules.First().Id;

                    var module = context.Modules.Skip(1).FirstOrDefault();
                    if (module != null) moduleId2 = module.Id;
                    else moduleId2 = moduleId1;

                    module = context.Modules.Skip(2).FirstOrDefault();
                    if (module != null) moduleId3 = module.Id;
                    else moduleId3 = moduleId1;
                }
                #endregion

                #region Add Videos if they don't already exist
                if (!context.Videos.Any())
                {
                    var videos = new List<Video>
                    {
                        new Video { ModuleId = moduleId1, CourseId = courseId1,
                            Title = "Video 1 Title",
                            Description = description.Substring(1, 35),
                            Duration = 50, Thumbnail = "/images/video1.jpg",
                            Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
                        },
                        new Video { ModuleId = moduleId1, CourseId = courseId1,
                            Title = "Video 2 Title",
                            Description = description.Substring(5, 35),
                            Duration = 45, Thumbnail = "/images/video2.jpg",
                            Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
                        },
                        new Video { ModuleId = moduleId1, CourseId = courseId1,
                            Title = "Video 3 Title",
                            Description = description.Substring(10, 35),
                            Duration = 41, Thumbnail = "/images/video3.jpg",
                            Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
                        },
                        new Video { ModuleId = moduleId3, CourseId = courseId2,
                            Title = "Video 4 Title",
                            Description = description.Substring(15, 35),
                            Duration = 41, Thumbnail = "/images/video4.jpg",
                            Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
                        },
                        new Video { ModuleId = moduleId2, CourseId = courseId1,
                            Title = "Video 5 Title",
                            Description = description.Substring(20, 35),
                            Duration = 42, Thumbnail = "/images/video5.jpg",
                            Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
                        }
                    };
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }
                #endregion

                #region Add Downloads if they don't already exist
                if (!context.Downloads.Any())
                {
                    var downloads = new List<Download>
                    {
                        new Download{ModuleId = moduleId1, CourseId = courseId1,
                            Title = "ADO.NET 1 (PDF)", Url = "https://some-url" },

                        new Download{ModuleId = moduleId1, CourseId = courseId1,
                            Title = "ADO.NET 2 (PDF)", Url = "https://some-url" },

                        new Download{ModuleId = moduleId3, CourseId = courseId2,
                            Title = "ADO.NET 1 (PDF)", Url = "https://some-url" }
                    };

                    context.Downloads.AddRange(downloads);
                    context.SaveChanges();
                }
                #endregion
            }
        }
    }
}

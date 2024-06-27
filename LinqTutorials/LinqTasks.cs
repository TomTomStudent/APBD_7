using LinqTutorials.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqTutorials
{
    public static class LinqTasks
    {
        public static IEnumerable<Emp> Emps { get; set; }
        public static IEnumerable<Dept> Depts { get; set; }

        static LinqTasks()
        {
            var empsCol = new List<Emp>();
            var deptsCol = new List<Dept>();

            #region Load depts

            var d1 = new Dept
            {
                Deptno = 1,
                Dname = "Finance",
                Loc = "Warsaw"
            };

            var d2 = new Dept
            {
                Deptno = 2,
                Dname = "IT",
                Loc = "Tokyo"
            };

            var d3 = new Dept
            {
                Deptno = 3,
                Dname = "AI",
                Loc = "Helsinki"
            };

            deptsCol.Add(d1);
            deptsCol.Add(d2);
            deptsCol.Add(d3);
            #endregion

            #region Load emps
            var e1 = new Emp
            {
                Deptno = 1,
                Empno = 1,
                Ename = "John Doe",
                HireDate = DateTime.Now.AddYears(-1),
                Job = "Backend programmer",
                Mgr = null,
                Salary = 4000
            };
            var e2 = new Emp
            {
                Deptno = 1,
                Empno = 2,
                Ename = "Lucas Vasquez",
                HireDate = DateTime.Now.AddYears(-3),
                Job = "Backend programmer",
                Mgr = null,
                Salary = 8000
            };
            var e3 = new Emp
            {
                Deptno = 1,
                Empno = 3,
                Ename = "Simon Smart",
                HireDate = DateTime.Now.AddYears(-1),
                Job = "Frontend programmer",
                Mgr = e1,
                Salary = 4000
            };
            var e4 = new Emp
            {
                Deptno = 2,
                Empno = 4,
                Ename = "Stacy Adams",
                HireDate = DateTime.Now.AddYears(-1),
                Job = "Frontend programmer",
                Mgr = null,
                Salary = 12000
            };
            var e5 = new Emp
            {
                Deptno = 2,
                Empno = 5,
                Ename = "Suzzane Smith",
                HireDate = DateTime.Now.AddYears(-1),
                Job = "Frontend programmer",
                Mgr = e4,
                Salary = 6000
            };
            var e6 = new Emp
            {
                Deptno = 3,
                Empno = 6,
                Ename = "Tim Alone",
                HireDate = DateTime.Now.AddYears(-1),
                Job = "Backend programmer",
                Mgr = null,
                Salary = 10
            };

            empsCol.Add(e1);
            empsCol.Add(e2);
            empsCol.Add(e3);
            empsCol.Add(e4);
            empsCol.Add(e5);
            empsCol.Add(e6);
            Emps = empsCol;

            #endregion

        }

        /// <summary>
        ///     SELECT * FROM Emps WHERE Job = "Backend programmer";
        /// </summary>
        public static IEnumerable<Emp> Task1()
        {
            IEnumerable<Emp> result = Emps
                .Where(e => e.Job == "Backend programmer");
            return result;
        }

        /// <summary>
        ///     SELECT * FROM Emps Job = "Frontend programmer" AND Salary>1000 ORDER BY Ename DESC;
        /// </summary>
        public static IEnumerable<Emp> Task2()
        {
            IEnumerable<Emp> result = Emps
                .Where(e => e.Job == "Frontend programmer" && e.Salary > 1000)
                .OrderByDescending(e => e.Ename);
            return result;
        }


        /// <summary>
        ///     SELECT MAX(Salary) FROM Emps;
        /// </summary>
        public static int Task3()
        {
            int result = Emps.
                Max(e => e.Salary);
            return result;
        }

        /// <summary>
        ///     SELECT * FROM Emps WHERE Salary=(SELECT MAX(Salary) FROM Emps);
        /// </summary>
        public static IEnumerable<Emp> Task4()
        {
            IEnumerable<Emp> result = Emps
                .Where(e => e.Salary == Emps.Max(f => f.Salary));
            return result;
        }

        /// <summary>
        ///    SELECT ename AS Nazwisko, job AS Praca FROM Emps;
        /// </summary>
        public static IEnumerable<object> Task5()
        {
            IEnumerable<object> result = Emps.
                    Select(e => new { Nazwisko = e.Ename, Praca = e.Job });
            return result;
        }

        /// <summary>
        ///     SELECT Emps.Ename, Emps.Job, Depts.Dname FROM Emps
        ///     INNER JOIN Depts ON Emps.Deptno=Depts.Deptno
        ///     Rezultat: Złączenie kolekcji Emps i Depts.
        /// </summary>
        public static IEnumerable<object> Task6()
        {
            IEnumerable<object> result = from emp in Emps
                                         join dept in Depts on emp.Deptno equals dept.Deptno
                                         select new { emp.Ename, emp.Job, dept.Dname };
            return result;
        }

        /// <summary>
        ///     SELECT Job AS Praca, COUNT(1) LiczbaPracownikow FROM Emps GROUP BY Job;
        /// </summary>
        public static IEnumerable<object> Task7()
        {
            IEnumerable<object> result = from emp in Emps
                                         group emp by emp.Job into jobGroup
                                         select new { Praca = jobGroup.Key, LiczbaPracownikow = jobGroup.Count() };
            return result;
        }

        /// <summary>
        ///     Return the value "true" if at least one
        ///     of the elements in the collection works as a "Backend programmer".
        /// </summary>
        public static bool Task8()
        {
            bool result = Emps.
                Any(e => e.Job == "Backend programmer");
            return result;
        }

        /// <summary>
        ///     SELECT TOP 1 * FROM Emp WHERE Job="Frontend programmer"
        ///     ORDER BY HireDate DESC;
        /// </summary>
        public static Emp Task9()
        {
            Emp result = Emps.Where(e => e.Job == "Frontend programmer").
                    OrderByDescending(e => e.HireDate)
                    .FirstOrDefault();
            return result;
        }

        /// <summary>
        ///     SELECT Ename, Job, Hiredate FROM Emps
        ///     UNION
        ///     SELECT "Brak wartości", null, null;
        /// </summary>
        public static IEnumerable<object> Task10()
        {
            var defaultObject = new { Ename = "Brak wartości", Job = (string)null, HireDate = (DateTime?)null };

            IEnumerable<object> result = Emps
                .Select(e => new { e.Ename, e.Job, e.HireDate })
                .Concat(new[] { defaultObject });
            return result;
        }

        /// <summary>
        /// Using LINQ, retrieve employees divided by departments, keeping in mind that:
        /// 1. We are only interested in departments with more than 1 employee
        /// 2. We want to return a list of objects with the following structure:
        ///    [
        ///      {name: "RESEARCH", numOfEmployees: 3},
        ///      {name: "SALES", numOfEmployees: 5},
        ///      ...
        ///    ]
        /// 3. Use anonymous types
        /// </summary>
        public static IEnumerable<object> Task11()
        {
            IEnumerable<object> result = from emp in Emps
                                         group emp by emp.Deptno into deptGroup
                                         join dept in Depts on deptGroup.Key equals dept.Deptno
                                         where deptGroup.Count() > 1
                                         select new { name = dept.Dname.ToUpper(), numOfEmployees = deptGroup.Count() }; ;
            return result;
        }

        /// <summary>
        /// Write your own extension method that will allow the following code snippet to compile.
        /// Add the method to the CustomExtensionMethods class, which is defined below.
        ///
        /// The method should return only those employees who have at least 1 direct subordinate.
        /// Employees should be sorted within the collection by surname (ascending) and salary (descending).
        /// </summary>
        public static IEnumerable<Emp> Task12()
        {
            IEnumerable<Emp> result =  Emps.GetEmpsWithSubordinates();
            return result;
        }

        /// <summary>
        /// The method below should return a single int number.
        /// It takes a list of integers as input.
        /// Try to find, using LINQ, the number that appears an odd number of times in the array of ints.
        /// It is assumed that there will always be one such number.
        /// For example: {1,1,1,1,1,1,10,1,1,1,1} => 10
        /// </summary>
        public static int Task13(int[] arr)
        {
            int result = arr
                .GroupBy(x => x)
                .Single(group => group.Count() % 2 != 0)
                .Key;
            return result;
        }

        /// <summary>
        /// Return only those departments that have exactly 5 employees or no employees at all.
        /// Sort the result by department name in ascending order.
        /// </summary>
        public static IEnumerable<Dept> Task14()
        {
            IEnumerable<Dept> result = Depts.
                Where(dept => Emps.All(emp => emp.Deptno != dept.Deptno) || Emps.
                    Count(emp => emp.Deptno == dept.Deptno) == 5)
                .OrderBy(dept => dept.Dname);
            return result;
        }

        /// <summary>
        ///     SELECT Job AS Praca, COUNT(1) LiczbaPracownikow FROM Emps
        ///     WHERE Job LIKE '%A%'
        ///     GROUP BY Job
        ///     HAVING COUNT(*)>2
        ///     ORDER BY COUNT(*) DESC;
        /// </summary>
        public static IEnumerable<object> Task15()
        {
            IEnumerable<object> result = Emps
                .Where(e => e.Job.Contains("A"))
                .GroupBy(e => e.Job)
                .Where(g => g.Count() > 2)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    Praca = g.Key,
                    LiczbaPracownikow = g.Count()
                });
            return result;
        }

        /// <summary>
        ///     SELECT * FROM Emps, Depts;
        /// </summary>
        public static IEnumerable<object> Task16()
        {
            IEnumerable<object> result = from emp in Emps
                                         from dept in Depts
                                         select new
                                         {
                                             emp,
                                             dept
                                         };
            return result;
        }
    }

    public static class CustomExtensionMethods
    {
        //Put your extension methods here
        public static IEnumerable<Emp> GetEmpsWithSubordinates(this IEnumerable<Emp> emps)
        {
            var result = emps.Where(e => emps.Any(e2 => e2.Mgr == e.Mgr)).OrderBy(e => e.Ename).ThenByDescending(e => e.Salary);
            return result;
        }

    }
}

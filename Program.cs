using Microsoft.Data.SqlClient;
using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;
using StudentCourseManagement.Services;

namespace StudentCourseManagement;

public class Program
{
    public static async Task Main()
    {
        var departmentRepository =
            new DepartmentRepository();

        var studentRepository =
            new StudentRepository();

        var courseRepository =
            new CourseRepository();

        var enrollmentRepository =
            new EnrollmentRepository();

        var auditLogRepository =
            new AuditLogRepository();

        var departmentService =
            new DepartmentService(departmentRepository);

        var studentService =
            new StudentService(studentRepository);

        var courseService =
            new CourseService(courseRepository);

        var enrollmentService =
            new EnrollmentService(enrollmentRepository);

        var auditLogService =
            new AuditLogService(auditLogRepository);

        while (true)
        {
            try
            {
                Console.Clear();

                Console.WriteLine(
                    "==========================================");

                Console.WriteLine(
                    "     STUDENT COURSE MANAGEMENT SYSTEM");

                Console.WriteLine(
                    "==========================================");

                Console.WriteLine("1. Department Management");
                Console.WriteLine("2. Student Management");
                Console.WriteLine("3. Course Management");
                Console.WriteLine("4. Enrollment Management");
                Console.WriteLine("5. View Audit Logs");
                Console.WriteLine("6. Exit");

                Console.WriteLine(
                    "==========================================");

                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        await DepartmentMenu(
                            departmentService,
                            auditLogService);
                        break;

                    case "2":
                        await StudentMenu(
                            studentService,
                            auditLogService);
                        break;

                    case "3":
                        await CourseMenu(
                            courseService,
                            auditLogService);
                        break;

                    case "4":
                        await EnrollmentMenu(
                            enrollmentService,
                            studentService,
                            courseService);
                        break;

                    case "5":
                        await ShowAuditLogs(
                            auditLogService);
                        break;

                    case "6":
                        Console.WriteLine(
                            "Application closed.");

                        return;

                    default:
                        Console.WriteLine(
                            "Invalid choice.");

                        Pause();
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Validation error: {ex.Message}");

                Pause();
            }
            catch (SqlException ex)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Database error: {ex.Message}");

                Pause();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"Unexpected error: {ex.Message}");

                Pause();
            }
        }
    }

    // ==============================
    // DEPARTMENT MENU
    // ==============================

    private static async Task DepartmentMenu(
        DepartmentService service,
        AuditLogService auditService)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine(
                "========== DEPARTMENT MANAGEMENT ==========");

            Console.WriteLine("1. Add Department");
            Console.WriteLine("2. View All Departments");
            Console.WriteLine("3. Search Department By ID");
            Console.WriteLine("4. Update Department");
            Console.WriteLine("5. Deactivate Department");
            Console.WriteLine("6. Back");

            Console.Write("Enter choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    {
                        Console.Write("Department name: ");
                        string name = Console.ReadLine() ?? "";

                        Console.Write("Location: ");
                        string location =
                            Console.ReadLine() ?? "";

                        await service.AddAsync(
                            name,
                            location);

                        Console.WriteLine(
                            "Department added successfully.");

                        Pause();
                        break;
                    }

                case "2":
                    {
                        var departments =
                            await service.GetAllAsync();

                        PrintDepartments(departments);

                        Pause();
                        break;
                    }

                case "3":
                    {
                        int id = ReadInt(
                            "Department ID: ");

                        var department =
                            await service.GetByIdAsync(id);

                        if (department is null)
                        {
                            Console.WriteLine(
                                "Department not found.");
                        }
                        else
                        {
                            PrintDepartment(department);
                        }

                        Pause();
                        break;
                    }

                case "4":
                    {
                        int id = ReadInt(
                            "Department ID: ");

                        Console.Write("New name: ");
                        string name =
                            Console.ReadLine() ?? "";

                        Console.Write("New location: ");
                        string location =
                            Console.ReadLine() ?? "";

                        await service.UpdateAsync(
                            id,
                            name,
                            location);

                        await auditService.AddAsync(
                            "Department",
                            id,
                            "UPDATED",
                            "Department updated");

                        Console.WriteLine(
                            "Department updated successfully.");

                        Pause();
                        break;
                    }

                case "5":
                    {
                        int id = ReadInt(
                            "Department ID: ");

                        await service.DeactivateAsync(id);

                        await auditService.AddAsync(
                            "Department",
                            id,
                            "DEACTIVATED",
                            "Department deactivated");

                        Console.WriteLine(
                            "Department deactivated successfully.");

                        Pause();
                        break;
                    }

                case "6":
                    return;

                default:
                    Console.WriteLine(
                        "Invalid choice.");

                    Pause();
                    break;
            }
        }
    }

    // ==============================
    // STUDENT MENU
    // ==============================

    private static async Task StudentMenu(
        StudentService service,
        AuditLogService auditService)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine(
                "============ STUDENT MANAGEMENT ============");

            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View All Students");
            Console.WriteLine("3. Search Student By ID");
            Console.WriteLine("4. Search Student By Name");
            Console.WriteLine("5. Update Student");
            Console.WriteLine("6. Deactivate Student");
            Console.WriteLine("7. Back");

            Console.Write("Enter choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    {
                        Console.Write("Student name: ");
                        string name =
                            Console.ReadLine() ?? "";

                        Console.Write("Email: ");
                        string email =
                            Console.ReadLine() ?? "";

                        int age = ReadInt("Age: ");

                        int departmentId =
                            ReadInt("Department ID: ");

                        await service.AddAsync(
                            name,
                            email,
                            age,
                            departmentId);

                        Console.WriteLine(
                            "Student added successfully.");

                        Pause();
                        break;
                    }

                case "2":
                    {
                        var students =
                            await service.GetAllAsync();

                        PrintStudents(students);

                        Pause();
                        break;
                    }

                case "3":
                    {
                        int id =
                            ReadInt("Student ID: ");

                        var student =
                            await service.GetByIdAsync(id);

                        if (student is null)
                        {
                            Console.WriteLine(
                                "Student not found.");
                        }
                        else
                        {
                            PrintStudent(student);
                        }

                        Pause();
                        break;
                    }

                case "4":
                    {
                        Console.Write("Student name: ");
                        string name =
                            Console.ReadLine() ?? "";

                        var students =
                            await service.SearchByNameAsync(
                                name);

                        PrintStudents(students);

                        Pause();
                        break;
                    }

                case "5":
                    {
                        int id =
                            ReadInt("Student ID: ");

                        Console.Write("Name: ");
                        string name =
                            Console.ReadLine() ?? "";

                        Console.Write("Email: ");
                        string email =
                            Console.ReadLine() ?? "";

                        int age =
                            ReadInt("Age: ");

                        int departmentId =
                            ReadInt("Department ID: ");

                        await service.UpdateAsync(
                            id,
                            name,
                            email,
                            age,
                            departmentId);

                        await auditService.AddAsync(
                            "Student",
                            id,
                            "UPDATED",
                            "Student updated");

                        Console.WriteLine(
                            "Student updated successfully.");

                        Pause();
                        break;
                    }

                case "6":
                    {
                        int id =
                            ReadInt("Student ID: ");

                        await service.DeactivateAsync(id);

                        await auditService.AddAsync(
                            "Student",
                            id,
                            "DEACTIVATED",
                            "Student deactivated");

                        Console.WriteLine(
                            "Student deactivated successfully.");

                        Pause();
                        break;
                    }

                case "7":
                    return;

                default:
                    Console.WriteLine(
                        "Invalid choice.");

                    Pause();
                    break;
            }
        }
    }

    // ==============================
    // COURSE MENU
    // ==============================

    private static async Task CourseMenu(
        CourseService service,
        AuditLogService auditService)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine(
                "============= COURSE MANAGEMENT =============");

            Console.WriteLine("1. Add Course");
            Console.WriteLine("2. View All Courses");
            Console.WriteLine("3. Search Course By ID");
            Console.WriteLine("4. Search Course By Name");
            Console.WriteLine("5. Update Course");
            Console.WriteLine("6. Deactivate Course");
            Console.WriteLine("7. Back");

            Console.Write("Enter choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    {
                        Console.Write("Course name: ");
                        string name =
                            Console.ReadLine() ?? "";

                        int duration =
                            ReadInt("Duration in months: ");

                        decimal fee =
                            ReadDecimal("Fee: ");

                        int departmentId =
                            ReadInt("Department ID: ");

                        await service.AddAsync(
                            name,
                            duration,
                            fee,
                            departmentId);

                        Console.WriteLine(
                            "Course added successfully.");

                        Pause();
                        break;
                    }

                case "2":
                    {
                        var courses =
                            await service.GetAllAsync();

                        PrintCourses(courses);

                        Pause();
                        break;
                    }

                case "3":
                    {
                        int id =
                            ReadInt("Course ID: ");

                        var course =
                            await service.GetByIdAsync(id);

                        if (course is null)
                        {
                            Console.WriteLine(
                                "Course not found.");
                        }
                        else
                        {
                            PrintCourse(course);
                        }

                        Pause();
                        break;
                    }

                case "4":
                    {
                        Console.Write("Course name: ");
                        string name =
                            Console.ReadLine() ?? "";

                        var courses =
                            await service.SearchByNameAsync(
                                name);

                        PrintCourses(courses);

                        Pause();
                        break;
                    }

                case "5":
                    {
                        int id =
                            ReadInt("Course ID: ");

                        Console.Write("Course name: ");
                        string name =
                            Console.ReadLine() ?? "";

                        int duration =
                            ReadInt("Duration in months: ");

                        decimal fee =
                            ReadDecimal("Fee: ");

                        int departmentId =
                            ReadInt("Department ID: ");

                        await service.UpdateAsync(
                            id,
                            name,
                            duration,
                            fee,
                            departmentId);

                        await auditService.AddAsync(
                            "Course",
                            id,
                            "UPDATED",
                            "Course updated");

                        Console.WriteLine(
                            "Course updated successfully.");

                        Pause();
                        break;
                    }

                case "6":
                    {
                        int id =
                            ReadInt("Course ID: ");

                        await service.DeactivateAsync(id);

                        await auditService.AddAsync(
                            "Course",
                            id,
                            "DEACTIVATED",
                            "Course deactivated");

                        Console.WriteLine(
                            "Course deactivated successfully.");

                        Pause();
                        break;
                    }

                case "7":
                    return;

                default:
                    Console.WriteLine(
                        "Invalid choice.");

                    Pause();
                    break;
            }
        }
    }

    // ==============================
    // ENROLLMENT MENU
    // ==============================

    private static async Task EnrollmentMenu(
        EnrollmentService service,
        StudentService studentService,
        CourseService courseService)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine(
                "=========== ENROLLMENT MANAGEMENT ===========");

            Console.WriteLine("1. Enroll Student");
            Console.WriteLine("2. View All Enrollments");
            Console.WriteLine("3. View Student Enrollments");
            Console.WriteLine("4. View Course Enrollments");
            Console.WriteLine("5. Update Enrollment Status");
            Console.WriteLine("6. Back");

            Console.Write("Enter choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    {
                        int studentId =
                            ReadInt("Student ID: ");

                        int courseId =
                            ReadInt("Course ID: ");

                        var student =
                            await studentService.GetByIdAsync(
                                studentId);

                        if (student is null)
                        {
                            Console.WriteLine(
                                "Student not found.");

                            Pause();
                            break;
                        }

                        var course =
                            await courseService.GetByIdAsync(
                                courseId);

                        if (course is null)
                        {
                            Console.WriteLine(
                                "Course not found.");

                            Pause();
                            break;
                        }

                        int enrollmentId =
                            await service.EnrollAsync(
                                studentId,
                                courseId);

                        Console.WriteLine(
                            $"Enrollment successful. " +
                            $"Enrollment ID: {enrollmentId}");

                        Pause();
                        break;
                    }

                case "2":
                    {
                        var enrollments =
                            await service.GetAllAsync();

                        PrintEnrollments(enrollments);

                        Pause();
                        break;
                    }

                case "3":
                    {
                        int studentId =
                            ReadInt("Student ID: ");

                        var enrollments =
                            await service.GetByStudentAsync(
                                studentId);

                        PrintEnrollments(enrollments);

                        Pause();
                        break;
                    }

                case "4":
                    {
                        int courseId =
                            ReadInt("Course ID: ");

                        var enrollments =
                            await service.GetByCourseAsync(
                                courseId);

                        PrintEnrollments(enrollments);

                        Pause();
                        break;
                    }

                case "5":
                    {
                        int enrollmentId =
                            ReadInt("Enrollment ID: ");

                        Console.WriteLine(
                            "1. Enrolled");

                        Console.WriteLine(
                            "2. Completed");

                        Console.WriteLine(
                            "3. Cancelled");

                        Console.Write(
                            "Select status: ");

                        string statusChoice =
                            Console.ReadLine() ?? "";

                        string status =
                            statusChoice switch
                            {
                                "1" => "Enrolled",
                                "2" => "Completed",
                                "3" => "Cancelled",
                                _ => ""
                            };

                        if (status == "")
                        {
                            Console.WriteLine(
                                "Invalid status.");

                            Pause();
                            break;
                        }

                        await service.UpdateStatusAsync(
                            enrollmentId,
                            status);

                        Console.WriteLine(
                            "Enrollment status updated.");

                        Pause();
                        break;
                    }

                case "6":
                    return;

                default:
                    Console.WriteLine(
                        "Invalid choice.");

                    Pause();
                    break;
            }
        }
    }

    // ==============================
    // AUDIT LOGS
    // ==============================

    private static async Task ShowAuditLogs(
        AuditLogService service)
    {
        Console.Clear();

        Console.WriteLine(
            "================ AUDIT LOGS ================");

        var logs = await service.GetAllAsync();

        if (logs.Count == 0)
        {
            Console.WriteLine(
                "No audit logs found.");
        }
        else
        {
            foreach (var log in logs)
            {
                Console.WriteLine(
                    $"{log.AuditId} | " +
                    $"{log.EntityName} | " +
                    $"{log.EntityId} | " +
                    $"{log.Action} | " +
                    $"{log.Details} | " +
                    $"{log.CreatedAt}");
            }
        }

        Pause();
    }

    // ==============================
    // PRINT METHODS
    // ==============================

    private static void PrintDepartments(
        List<Department> departments)
    {
        Console.WriteLine();

        if (departments.Count == 0)
        {
            Console.WriteLine(
                "No departments found.");

            return;
        }

        foreach (var d in departments)
        {
            Console.WriteLine(
                $"{d.DepartmentId} | " +
                $"{d.DepartmentName} | " +
                $"{d.Location} | " +
                $"Active: {d.IsActive}");
        }
    }

    private static void PrintDepartment(
        Department d)
    {
        Console.WriteLine();
        Console.WriteLine(
            $"ID: {d.DepartmentId}");

        Console.WriteLine(
            $"Name: {d.DepartmentName}");

        Console.WriteLine(
            $"Location: {d.Location}");

        Console.WriteLine(
            $"Active: {d.IsActive}");
    }

    private static void PrintStudents(
        List<Student> students)
    {
        Console.WriteLine();

        if (students.Count == 0)
        {
            Console.WriteLine(
                "No students found.");

            return;
        }

        foreach (var s in students)
        {
            Console.WriteLine(
                $"{s.StudentId} | " +
                $"{s.Name} | " +
                $"{s.Email} | " +
                $"Age: {s.Age} | " +
                $"Dept: {s.DepartmentId} | " +
                $"Admission: {s.AdmissionDate:d} | " +
                $"Active: {s.IsActive}");
        }
    }

    private static void PrintStudent(Student s)
    {
        Console.WriteLine();
        Console.WriteLine(
            $"ID: {s.StudentId}");

        Console.WriteLine(
            $"Name: {s.Name}");

        Console.WriteLine(
            $"Email: {s.Email}");

        Console.WriteLine(
            $"Age: {s.Age}");

        Console.WriteLine(
            $"Department ID: {s.DepartmentId}");

        Console.WriteLine(
            $"Admission Date: {s.AdmissionDate}");

        Console.WriteLine(
            $"Active: {s.IsActive}");
    }

    private static void PrintCourses(
        List<Course> courses)
    {
        Console.WriteLine();

        if (courses.Count == 0)
        {
            Console.WriteLine(
                "No courses found.");

            return;
        }

        foreach (var c in courses)
        {
            Console.WriteLine(
                $"{c.CourseId} | " +
                $"{c.CourseName} | " +
                $"{c.DurationMonths} months | " +
                $"Fee: {c.Fee:N2} | " +
                $"Dept: {c.DepartmentId} | " +
                $"Active: {c.IsActive}");
        }
    }

    private static void PrintCourse(Course c)
    {
        Console.WriteLine();
        Console.WriteLine(
            $"ID: {c.CourseId}");

        Console.WriteLine(
            $"Name: {c.CourseName}");

        Console.WriteLine(
            $"Duration: {c.DurationMonths} months");

        Console.WriteLine(
            $"Fee: {c.Fee:N2}");

        Console.WriteLine(
            $"Department ID: {c.DepartmentId}");

        Console.WriteLine(
            $"Active: {c.IsActive}");
    }

    private static void PrintEnrollments(
        List<Enrollment> enrollments)
    {
        Console.WriteLine();

        if (enrollments.Count == 0)
        {
            Console.WriteLine(
                "No enrollments found.");

            return;
        }

        foreach (var e in enrollments)
        {
            Console.WriteLine(
                $"{e.EnrollmentId} | " +
                $"Student: {e.StudentId} | " +
                $"Course: {e.CourseId} | " +
                $"{e.EnrolledAt} | " +
                $"Status: {e.Status}");
        }
    }

    // ==============================
    // INPUT HELPERS
    // ==============================

    private static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(
                    Console.ReadLine(),
                    out int value))
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid number.");
        }
    }

    private static decimal ReadDecimal(
        string message)
    {
        while (true)
        {
            Console.Write(message);

            if (decimal.TryParse(
                    Console.ReadLine(),
                    out decimal value))
            {
                return value;
            }

            Console.WriteLine(
                "Please enter a valid amount.");
        }
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine(
            "Press ENTER to continue...");

        Console.ReadLine();
    }
}
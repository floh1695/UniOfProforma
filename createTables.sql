use [University]
go

set ansi_nulls on
go

set quoted_identifier on
go

/*
 * Drop all tables
 * - Order matters, you cannot delete tables depended on by other tables
 */
drop table [Jobs]
drop table [Enrollment]
drop table [Courses]
drop table [Professors]
drop table [Students]

/*
 * Students
 */
create table [Students](
	[ID] int IDENTITY(1,1) not null,
	[FullName] nvarchar(50) not null,
	[Email] nvarchar(50) not null,
	[PhoneNumber] nvarchar(50) not null,
	[Major] nvarchar(50) not null,
 constraint [PK_Students] primary key clustered 
(
	[ID] asc
)with (pad_index = off, statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on) on [PRIMARY]
) on [PRIMARY]
go

/*
 * Professors
 */
create table [Professors](
	[ID] int IDENTITY(1,1) not null,
	[Name] nvarchar(50) not null,
	[Title] nvarchar(50) not null,
 constraint [PK_Professors] primary key clustered 
(
	[ID] asc
)with (pad_index = off, statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on) on [PRIMARY]
) on [PRIMARY]
go

/*
 * Courses
 */
create table [Courses](
	[ID] int IDENTITY(1,1) not null,
	[Number] nvarchar(50) not null,
	[Level] int not null,
	[Name] nvarchar(50) not null,
	[Room] nvarchar(50) not null,
	[StartTime] time not null,
 constraint [PK_Courses] primary key clustered 
(
	[ID] asc
)with (pad_index = off, statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on) on [PRIMARY]
) on [PRIMARY]
go

/*
 * Enrollment
 */
create table [Enrollment](
	[ID] int IDENTITY(1,1) not null,
	[StudentID] int not null,
	[CourseID] int not null,
 constraint [PK_Enrollment] primary key clustered 
(
	[ID] asc
)with (pad_index = off, statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on) on [PRIMARY]
) on [PRIMARY]
go

-- Enrollment Foreign Keys
alter table [Enrollment]  with check add constraint [FK_Enrollment_Students] foreign key([StudentID])
references [Students] ([ID])
go
alter table [Enrollment] check constraint [FK_Enrollment_Students]
go

alter table [Enrollment]  with check add constraint [FK_Enrollment_Courses] foreign key([CourseID])
references [Courses] ([ID])
go
alter table [Enrollment] check constraint [FK_Enrollment_Courses]
go

/*
 * Jobs
 */
create table [Jobs](
	[ID] int IDENTITY(1,1) not null,
	[ProfessorID] int not null,
	[CourseID] int not null,
 constraint [PK_Jobs] primary key clustered 
(
	[ID] asc
)with (pad_index = off, statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on) on [PRIMARY]
) on [PRIMARY]
go

-- Jobs Foriegn keys
alter table [Jobs]  with check add constraint [FK_Jobs_Professors] foreign key([ProfessorID])
references [Professors] ([ID])
go
alter table [Jobs] check constraint [FK_Jobs_Professors]
go

alter table [Jobs]  with check add constraint [FK_Jobs_Courses] foreign key([CourseID])
references [Courses] ([ID])
go
alter table [Jobs] check constraint [FK_Jobs_Courses]
go

/*
 *
 * Fill tables in with garbage data
 *
 */

 insert
	into [Students]
		(FullName, Email, PhoneNumber, Major)
	values
		('Arnold Zook', 'az@pu.fake.edu', '555-555-0100', 'Art'),
		('Banner Ayemen', 'ba@pu.fake.edu', '555-555-0101', 'Music'),
		('Crinn Book', 'cb@pu.fake.edu', '555-555-0102', 'Math'),
		('Dover Cliff', 'dc@pu.fake.edu', '555-555-0103', 'English'),
		('Ellie Deli', 'ed@pu.fake.edu', '555-555-0104', 'Biology'),
		('Fann Elign', 'fe@pu.fake.edu', '555-555-0105', 'Stats'),
		('George Foot', 'gf@pu.fake.edu', '555-555-0106', 'American Sign Language'),
		('Howard Goose', 'hg@pu.fake.edu', '555-555-0107', 'Computer Science'),
		('Innie Homes', 'ih@pu.fake.edu', '555-555-0108', 'Business'),
		('Jilly Illy', 'ji@pu.fake.edu', '555-555-0109', 'Physics')
go

insert
	into [Professors]
		([Name], Title)
	values
		('Farnsworth', 'Dr'),
		('Venkman', 'Dr'),
		('Layton', 'Prof'),
		('Kiss', 'Dr')
go

insert
	into [Courses]
		(Number, [Level], [Name], Room, StartTime)
	values
		('MA-101', 1,
			'Algebra', 'S-301', '08:00'),
		('MU-110A', 1,
			'Intro Piano - Part 1', 'A-103', '08:00'),
		('MA-156', 1,
			'Single Variable Calculus', 'S-242', '11:00'),
		('MA204', 2,
			'Multivariable Calculus', 'S-236', '14:00'),
		('MU-110B', 1,
			'Intro Piano - Part 2', 'A-103', '11:00'),
		('CS-101', 1,
			'Programming VB/Excel', 'S-213', '09:30'),
		('CS-145', 1,
			'Programming C/C++', 'S-213', '12:45'),
		('CS-207', 2,
			'Data Structures/Algorithms', 'S418', '16:00'),
		('PH-715', 5,
			'Philosophy of Meta Philosophy', 'A412', '18:00')
go

insert
	into [Enrollment]
		(StudentID, CourseID)
	values
		((select top(1) ID from [Students] order by newid()),
			(select top(1) ID from [Courses] order by newid()))
go 20 --Hack for looping, good for testing

insert
	into [Jobs]
		(ProfessorID, CourseID)
	values
		((select top(1) ID from [Professors] order by newid()),
			(select top(1) ID from [Courses] order by newid()))
go 10 --Hack for looping, good for testing

/*
 * Find all non Expired Leases
 */
/*
select *
	from [dbo].[Lease]
	where [ExpirationDate] > convert(date, getdate())
go
*/

/*
 * Select the email of all favored customers
 */
/*
select [Email]
	from [dbo].[Customer]
	where [FavoredCustomer] = 1
go
*/

/*
 * Show me the make, model, color, availability, and Miles Driven for all Cars
 */
/*
select [Make], [Model], [Color], [AvailableForLease], [MilesDriven]
	from [dbo].[Car] as Car
		join [dbo].[CarData] as CarData
			on Car.CarDataID = CarData.ID
go
*/

/*
 * All the emails of Customers that have expired leases
 */
/*
select [Email]
	from [dbo].[Contract] as [Contract]
		join [dbo].[Customer] as Customer
			on [Contract].CustomerID = Customer.ID
		join [dbo].[Lease] as Lease
			on [Contract].LeaseID = Lease.ID
	where
		Lease.ExpirationDate < convert(date, getdate())
go
*/
		
/*
 * The make and Model of the cars that are leased out
 */
/*
select [Make], [Model]
	from [dbo].[Lease] as Lease
		join [dbo].[Car] as Car
			on Lease.CarID = Car.ID
		join [dbo].[CarData] as CarData
			on Car.CarDataID = CarData.ID
go
*/

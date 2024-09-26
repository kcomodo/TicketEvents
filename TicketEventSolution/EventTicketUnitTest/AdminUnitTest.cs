using TicketEventBackEnd.Repositories.Admin;
using Moq;
using TicketEventBackEnd.Models.Admin;
namespace EventTicketUnitTest
{
    public class AdminUnitTest
    {
        [Fact]
        public void getAllAdmin_True()
        {
            //create a mockRepository for the adminRepository
            var mockRepository = new Mock<IAdminRepository>();
            var adminList = new List<AdminModel>
            {
                new AdminModel
                {
                    admin_id = 1,
                    admin_email = "testing@gmail.com",
                    admin_password = "testing1234"
                },
                new AdminModel
                {
                    admin_id = 2,
                    admin_email = "admin2@gmail.com",
                    admin_password = "password2"
                }
            };

            // Setup mock to return the list when getAllAdmin is called
            mockRepository.Setup(x => x.getAllAdmin()).Returns(adminList);

            // Act
            var results = mockRepository.Object.getAllAdmin();

            // Assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count()); // Check the number of items returned
            Assert.Equal("testing@gmail.com", results.First().admin_email); // Check the first admin's email
            Assert.Equal("admin2@gmail.com", results.ElementAt(1).admin_email); // Check the second admin's email
        }
        [Fact]
        public void getAllAdmin_Empty()
        {
            var mockRepository = new Mock<IAdminRepository>();
            var adminList = new List<AdminModel>
            {
         
            };
            mockRepository.Setup(x => x.getAllAdmin()).Returns(adminList);

            // Act
            var results = mockRepository.Object.getAllAdmin();

            // Assert
            Assert.Empty(results);
           
        }
        [Fact]
        public void getAllAdmin_False()
        {
            //create a mockRepository for the adminRepository
            var mockRepository = new Mock<IAdminRepository>();
            var adminList = new List<AdminModel>
            {
                new AdminModel
                {
                    admin_id = 1,
                    admin_email = "testing@gmail.com",
                    admin_password = "testing1234"
                },
                new AdminModel
                {
                    admin_id = 2,
                    admin_email = "admin2@gmail.com",
                    admin_password = "password2"
                }
            };

            // Setup mock to return the list when getAllAdmin is called
            mockRepository.Setup(x => x.getAllAdmin()).Returns(adminList);

            // Act
            var results = mockRepository.Object.getAllAdmin();

            // Assert
            Assert.NotNull(results);
            Assert.NotEqual(3, results.Count()); // Check the number of items returned
            Assert.NotEqual("falseTesting@gmail.com", results.First().admin_email); // Check the first admin's email
            Assert.NotEqual("falseAdmin2@gmail.com", results.ElementAt(1).admin_email); // Check the second admin's email
        }
        [Fact]
        public void getAdminInfo_True()
        {
            var mockRepository = new Mock<IAdminRepository>();
            var admin = new AdminModel()
            {
                admin_id = 1,
                admin_email = "testing@gmail.com",
                admin_password = "testing1234"
            };
            mockRepository.Setup(x => x.getAdminInfo("testing@gmail.com")).Returns(admin);
            var results = mockRepository.Object.getAdminInfo("testing@gmail.com");
            Assert.Equal(1, results.admin_id);
            Assert.Equal("testing@gmail.com", results.admin_email);
            Assert.Equal("testing1234", results.admin_password);
        }
        [Fact]
        public void getAdminInfo_Empty()
        {
            var mockRepository = new Mock<IAdminRepository>();
            var admin = new AdminModel()
            {
              
            };
            mockRepository.Setup(x => x.getAdminInfo("testing@gmail.com")).Returns(admin);
            var results = mockRepository.Object.getAdminInfo("testing@gmail.com");
            Assert.Equal(0, results.admin_id);
            Assert.Null(results.admin_email);
            Assert.Null(results.admin_password);
        }
        [Fact]
        public void getAdminInfo_False()
        {
            var mockRepository = new Mock<IAdminRepository>();
            var admin = new AdminModel()
            {
                admin_id = 1,
                admin_email = "testing@gmail.com",
                admin_password = "testing1234"
            };
            mockRepository.Setup(x => x.getAdminInfo("testing@gmail.com")).Returns(admin);
            var results = mockRepository.Object.getAdminInfo("testing@gmail.com");
            Assert.NotEqual(2, results.admin_id);
            Assert.NotEqual("falsetesting@gmail.com", results.admin_email);
            Assert.NotEqual("falsetesting1234", results.admin_password);
        }
        [Fact]
        public void addAdmin_True() 
        { 

        }
        [Fact]
        public void addAdmin_False()
        {

        }
        [Fact]
        public void deleteAdmin_True()
        {

        }
        [Fact]
        public void deleteAdmin_False()
        {

        }
        [Fact]
        public void updateAdmin_True()
        {

        }
        [Fact]
        public void updateAdmin_False()
        {

        }
    }
}
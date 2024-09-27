using TicketEventBackEnd.Repositories.Admin;
using Moq;
using TicketEventBackEnd.Models.Admin;
using MySqlX.XDevAPI.Common;
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
            var mockRepository = new Mock<IAdminRepository>();
            var adminEmail = "testing@gmail.com";
            var adminPassword = "testing1234";
            mockRepository.Setup(repo => repo.addAdmin(It.IsAny<String>(), It.IsAny<String>())).Callback<string,string>
                ((
                email, password) =>
                {
                    var admin = new AdminModel
                    {
                        admin_email = email,
                        admin_password = password
                    };
                    mockRepository.Setup(x => x.getAdminInfo(adminEmail)).Returns(admin);
                });
            mockRepository.Object.addAdmin(adminEmail, adminPassword);
            var result = mockRepository.Object.getAdminInfo(adminEmail);
            Assert.NotNull(result);
            Assert.Equal("testing@gmail.com", result.admin_email);
            Assert.Equal("testing1234", result.admin_password);
        }
        [Fact]
        public void addAdmin_False()
        {
            var mockRepository = new Mock<IAdminRepository>();
            var adminEmail = "testing@gmail.com";
            var adminPassword = "testing1234";
            mockRepository.Setup(repo => repo.addAdmin(It.IsAny<String>(), It.IsAny<String>())).Callback<string, string>
                ((
                email, password) =>
                {
                    var admin = new AdminModel
                    {
                        admin_email = email,
                        admin_password = password
                    };
                    mockRepository.Setup(x => x.getAdminInfo(adminEmail)).Returns(admin);
                });
            mockRepository.Object.addAdmin(adminEmail, adminPassword);
            var result = mockRepository.Object.getAdminInfo(adminEmail);
            Assert.NotNull(result);
            Assert.NotEqual("falsetesting@gmail.com", result.admin_email);
            Assert.NotEqual("falsetesting1234", result.admin_password);
        }
        [Fact]
        public void deleteAdmin_True()
        {
            var mockRepository = new Mock<IAdminRepository>();
            var admin = new AdminModel()
            {
                admin_id = 1,
                admin_email = "testing@gmail.com",
                admin_password = "testing1234"
            };
            mockRepository.Setup(x => x.getAdminInfo("testing@gmail.com")).Returns(admin);
            mockRepository.Setup(x => x.deleteAdmin("testing@gmail.com"));
            var fetchedAdmin = mockRepository.Object.getAdminInfo("testing@gmail.com");
            mockRepository.Object.deleteAdmin("testing@gmail.com");
            Assert.NotNull(fetchedAdmin);
            mockRepository.Verify(x => x.deleteAdmin("testing@gmail.com"), Times.Once);
        }
        [Fact]
        public void deleteAdmin_False()
        {
            var mockRepository = new Mock<IAdminRepository>();
            var admin = new AdminModel()
            {
                admin_id = 1,
                admin_email = "testing@gmail.com",
                admin_password = "testing1234"
            };
            mockRepository.Setup(x => x.getAdminInfo("testing@gmail.com")).Returns(admin);
            mockRepository.Setup(x => x.deleteAdmin("testing@gmail.com"))
                          .Throws(new Exception("Delete failed"));
            var fetchedAdmin = mockRepository.Object.getAdminInfo("testing@gmail.com");
            Assert.NotNull(fetchedAdmin);
            var ex = Assert.Throws<Exception>(() => mockRepository.Object.deleteAdmin("testing@gmail.com"));
            Assert.Equal("Delete failed", ex.Message);
            mockRepository.Verify(x => x.deleteAdmin("testing@gmail.com"), Times.Once);
        }
        [Fact]
        public void updateAdmin_True()
        {
            var mockRepository = new Mock<IAdminRepository>();
            var admin = new AdminModel()
            {
                admin_id = 1,
                admin_email = "testing@gmail.com",
                admin_password = "testing1234"
            };
            mockRepository.Setup(x => x.getAdminInfo("testing@gmail.com")).Returns(admin);
            mockRepository.Setup(x => x.updateAdmin(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string,string,string>((email,password,target) =>
                {
                    if(admin.admin_email == target)
                    {
                        admin.admin_email = email;
                        admin.admin_password = password;
                    }
                }
            );
            mockRepository.Object.updateAdmin("updateEmail@gmail.com", "updatePassword", "testing@gmail.com");
            var result = mockRepository.Object.getAdminInfo("testing@gmail.com");
            Assert.NotNull(result); 
            Assert.Equal("updateEmail@gmail.com", result.admin_email); 
            Assert.Equal("updatePassword", result.admin_password);
        }
        [Fact]
        public void updateAdmin_False()
        {
            var mockRepository = new Mock<IAdminRepository>();
            var admin = new AdminModel()
            {
                admin_id = 1,
                admin_email = "testing@gmail.com",
                admin_password = "testing1234"
            };
            mockRepository.Setup(x => x.getAdminInfo("testing@gmail.com")).Returns(admin);
            mockRepository.Setup(x => x.updateAdmin(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((email, password, target) =>
                {
                    if (admin.admin_email == target)
                    {
                        admin.admin_email = email;
                        admin.admin_password = password;
                    }
                }
            );
            mockRepository.Object.updateAdmin("updateEmail@gmail.com", "updatePassword", "testing@gmail.com");
            var result = mockRepository.Object.getAdminInfo("testing@gmail.com");
            Assert.NotNull(result);
            Assert.NotEqual("falseEmail@gmail.com", result.admin_email);
            Assert.NotEqual("falsePassword", result.admin_password);
        }
    }
}
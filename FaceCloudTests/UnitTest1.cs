using System;
using System.Collections.Generic;
using Xunit;
using FaceCloudApp;

namespace FaceCloudTests
{
    public class ModerationServiceTests
    {
        private readonly ModerationService _service = new ModerationService();

        // --- ТЕСТИ ДЛЯ AddWarning ---

        [Fact]
        public void AddWarning_NullUser_ThrowsArgumentNullException()
        {
            // Arrange (Недопустимий клас)
            User nullUser = null;

            // Act & Assert (Негативний тест)
            Assert.Throws<ArgumentNullException>(() => _service.AddWarning(nullUser));
        }

        [Fact]
        public void AddWarning_ValidUser_IncrementsWarnings()
        {
            // Arrange (Позитивний тест)
            var user = new User { Username = "Ilona", Warnings = 0, IsBanned = false };

            // Act
            _service.AddWarning(user);

            // Assert
            Assert.Equal(1, user.Warnings);
            Assert.False(user.IsBanned);
        }

        [Fact]
        public void AddWarning_ReachesThreeWarnings_BansUser()
        {
            // Arrange (Граничне значення перед баном)
            var user = new User { Username = "User_X", Warnings = 2, IsBanned = false };

            // Act
            _service.AddWarning(user);

            // Assert
            Assert.Equal(3, user.Warnings);
            Assert.True(user.IsBanned);
        }

        [Fact]
        public void AddWarning_AlreadyBannedUser_DoesNotChangeState()
        {
            // Arrange (Позитивний тест стану бану)
            var user = new User { Username = "BannedUser", Warnings = 3, IsBanned = true };

            // Act
            _service.AddWarning(user);

            // Assert
            Assert.Equal(3, user.Warnings);
            Assert.True(user.IsBanned);
        }

        // --- ТЕСТИ ДЛЯ CanPostContent ---

        [Fact]
        public void CanPostContent_NullUser_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert (Негативний тест)
            Assert.Throws<ArgumentNullException>(() => _service.CanPostContent(null, "Hello"));
        }

        [Fact]
        public void CanPostContent_BannedUser_ReturnsFalse()
        {
            // Arrange (Перевірка прав забаненого)
            var user = new User { IsBanned = true };

            // Act
            var result = _service.CanPostContent(user, "Some content");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanPostContent_EmptyContent_ReturnsFalse()
        {
            // Arrange (Межа пустого рядка)
            var user = new User { IsBanned = false };

            // Act
            var result = _service.CanPostContent(user, "   ");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanPostContent_TooLongContent_ReturnsFalse()
        {
            // Arrange (Межа 500 символів, тестуємо 501)
            var user = new User { IsBanned = false };
            string longContent = new string('A', 501);

            // Act
            var result = _service.CanPostContent(user, longContent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanPostContent_ValidContent_ReturnsTrue()
        {
            // Arrange (Стандартний допустимий клас)
            var user = new User { IsBanned = false };

            // Act
            var result = _service.CanPostContent(user, "Valid post for FaceCloud!");

            // Assert
            Assert.True(result);
        }

        // --- ТЕСТИ ДЛЯ MassUnban ---

        [Fact]
        public void MassUnban_NullList_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert (Негативний тест на колекцію)
            Assert.Throws<ArgumentNullException>(() => _service.MassUnban(null));
        }

        [Fact]
        public void MassUnban_ValidList_UnbansAllUsersAndReturnsCount()
        {
            // Arrange (Тестування логіки циклу)
            var users = new List<User>
            {
                new User { Username = "A", IsBanned = true, Warnings = 3 },
                new User { Username = "B", IsBanned = false, Warnings = 0 },
                new User { Username = "C", IsBanned = true, Warnings = 3 }
            };

            // Act
            int unbannedCount = _service.MassUnban(users);

            // Assert
            Assert.Equal(2, unbannedCount);
            Assert.False(users[0].IsBanned);
            Assert.Equal(0, users[0].Warnings);
            Assert.False(users[2].IsBanned);
        }
    }
}
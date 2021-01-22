using API.Helpers;
using API.Models;
using System;
using Xunit;

namespace Tests.ToDoItemTests
{
    public class MustAddOneToDoItemTest
    {
        [Fact]
        public void MustAddOneToDoItem()
        {
            //arrange
            var description = "Concluir api amanhã";
            var expiration = DateTime.Now.AddDays(1);
            var user = new User("joao@joao.com", "password", AccountHelper.DefaultUserRole);
            var newItem = new ToDoItem(description, expiration);

            //action
            user.AddToDoItem(newItem);

            //assert
            Assert.Single(user.ToDoItems);
            Assert.Equal(description, user.ToDoItems[0].Description);
            Assert.Equal(expiration, user.ToDoItems[0].Expiration);
        }
    }
}

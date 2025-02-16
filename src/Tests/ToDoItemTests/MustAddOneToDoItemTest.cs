using API.Helpers;
using Dominio.Entities;
using System;
using Xunit;

namespace Tests.ToDoItemTests
{
    public class MustAddOneToDoItemTest
    {
        [Fact]
        public void MustAddOneToDoItem()
        {
            var description = "Concluir api amanhã";
            var expiration = DateTime.Now.AddDays(1);
            var user = new User("joao@joao.com", "password", AccountConstants.DefaultUserRole);
            var newItem = new ToDoItem(description, expiration);

            user.AddToDoItem(newItem);

            Assert.Single(user.ToDoItems);
            Assert.Equal(description, user.ToDoItems[0].Description);
            Assert.Equal(expiration, user.ToDoItems[0].Expiration);
        }
    }
}

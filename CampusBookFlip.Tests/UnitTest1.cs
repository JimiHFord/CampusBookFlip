using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CampusBookFlip.Domain.Abstract;
using CampusBookFlip.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CampusBookFlip.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mock = new Mock<IRepository>();
            //IQueryable<Book> book_list = new List<Book>() { new Book { }, new Book { } };
            //mock.Setup(m => m.Book).Returns(book_list);
        }
    }
}

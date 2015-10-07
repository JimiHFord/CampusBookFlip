var express = require('express'),
    router = express.Router(),
    gbooks = require('../../services/gbooks/gbooks'),
    async = require('async'),
    mongoose = require('mongoose'),
    Book = mongoose.model('Book');

function findByIsbn(isbn, callback) {
  Book.findOne({'volumeInfo.industryIdentifiers.identifier': { $in : isbn } }, callback);
}

// TODO: fix error handling - it's not working
function handleQuery(req, res, next) {
  var query = req.param('query', 'null');

  gbooks.search(query, req.body, function(err, response, data) {
    var returnBooks = [];
    if(!err && response.statusCode == 200) {
      if(data && data.items) {
        async.each(data.items, function (book, callback) {
          var identifiers = [];
          var len = book && book.volumeInfo &&
          book.volumeInfo.industryIdentifiers ?
          book.volumeInfo.industryIdentifiers.length : 0;

          for(var i = 0; i < len; i++) {
            identifiers.push(book.volumeInfo.industryIdentifiers[i].identifier);
          }

          findByIsbn(identifiers, function (err, dbBook) {
            // If we didn't find a book in our database,
            // save this one
            if(dbBook == null) {
              console.log('found new book - saving...');
              book = new Book(book);
              book.save(function(err) {
                if(err) {
                  console.error(err);
                }
              });
              returnBooks.push(book);
            } else {
              console.log('ignoring duplicate');
              returnBooks.push(dbBook);
            }
            callback();
          });
        }, function(err) {
          if(err) {
            throw err;
          }
          res.json(returnBooks);
        });
      }
    } else {
      // console.log(err, response);
      throw err;
    }
  });
}


router.get('/search/:query?', handleQuery);

router.get('/test/:isbn?', function(req, res) {
  var isbn = req.param('isbn', null);
  if(isbn == null) {
    res.json(new Book());
  } else {
    findByIsbn([isbn], function(err, book) {
      if(!err) {
        if(book) {
          res.json(book);
        } else {
          res.json(new Book());
        }
      } else {
        throw err;
      }
    });
  }

});

module.exports = router;

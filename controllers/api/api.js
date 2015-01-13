var express = require('express'),
    api = express.Router(),
    books = require('./books'),
    colleges = require('./colleges');

api.use('/books', books);
api.use('/colleges', colleges);

module.exports = api;

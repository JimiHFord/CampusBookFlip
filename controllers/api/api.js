var express = require('express'),
    api = express.Router(),
    books = require('./books');

api.use('/books', books);

module.exports = api;

var express = require('express'),
    api = express.Router(),
    books = require('./books'),
    colleges = require('./colleges'),
    geography = require('./geography');

api.use('/books', books);
api.use('/colleges', colleges.router);
api.use('/geography', geography.router);

module.exports = api;

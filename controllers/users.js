var express = require('express'),
		router = express.Router(),
		Users = require('../models/User');


module.exports = function() {
	/* GET users listing. */
	router.get('/', function(req, res) {
		Users.find().lean().exec(function(err, items) {
			console.log(JSON.stringify(items));
			res.json(items);
		});
	});

	return router;
}

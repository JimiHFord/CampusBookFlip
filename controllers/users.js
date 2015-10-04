var express = require('express'),
		router = express.Router(),
		mongoose = require('mongoose'),
		Users = mongoose.model('User');


module.exports = function() {
	/* GET users listing. */
	router.get('/', function(req, res) {
		Users.find().lean().exec(function(err, items) {
			res.json(items);
		});
	});

	return router;
}

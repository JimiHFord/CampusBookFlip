var express = require('express'),
		router = express.Router(),
<<<<<<< HEAD
		Users = require('../models/User');
=======
		mongoose = require('mongoose'),
		Users = mongoose.model('User');
>>>>>>> feature/robustify-registration


module.exports = function() {
	/* GET users listing. */
	router.get('/', function(req, res) {
		Users.find().lean().exec(function(err, items) {
			res.json(items);
		});
	});

	return router;
}

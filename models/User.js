var mongoose = require('mongoose');
var model;

try {
	model = mongoose.model('User');
} catch (err) {
	model = mongoose.model('User', {
		username: String,
		password: String,
		email: String,
		gender: String,
		address: String
	});
}

module.exports = model;

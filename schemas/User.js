var mongoose = require('mongoose');

try {
	module.exports = mongoose.model('User');
} catch (err) {
	module.exports = mongoose.model('User', {
		username: String,
		password: String,
		email: String,
		gender: String,
		address: String
	});
}

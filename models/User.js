var mongoose = require('mongoose');
var model;

try {
	model = mongoose.model('User');
} catch (err) {
	model = mongoose.model('User', {
		oauthProviders: [{
			oauthID: Number,
			provider: String
		}],
		username: String,
		name: String,
		password: String,
		email: String,
		gender: String,
		address: String,
		colleges: [Number] // Institution_ID
	});
}

module.exports = model;

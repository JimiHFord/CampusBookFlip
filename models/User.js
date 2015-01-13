var mongoose = require('mongoose'),
		Schema = mongoose.Schema;


module.exports = function() {
	var schema = new Schema({
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
		colleges: [Number], // Institution_ID
		needsColleges: Boolean
	}, {collection: 'User'});
	mongoose.model('User', schema);
};

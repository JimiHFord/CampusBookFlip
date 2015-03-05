var mongoose = require('mongoose'),
		Schema = mongoose.Schema;


module.exports = function() {
	var schema = new Schema({
		// oauthProviders: [{
		// 	oauthID: Number,
		// 	provider: String
		// }],
		username: String,
		firstName: String,
		lastName: String,
		password: String,
		email: String,
		gender: String,
		address: String,
		colleges: [Number], // Institution_ID
		needsColleges: Boolean,
		facebook: {
			id: String,
			token: String
		},
		twitter: {
			id: String,
			token: String
		},
		google: {
			id: String,
			token: String
		}
	}, {collection: 'User'});
	mongoose.model('User', schema);
};

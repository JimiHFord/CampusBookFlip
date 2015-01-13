var mongoose = require('mongoose'),
    Schema = mongoose.Schema;

module.exports = function() {
  var schema = new Schema({
    Institution_ID: Number,
    Institution_Name: String,
    Institution_Address: String,
    Institution_City: String,
    Institution_State: String,
    Institution_Zip: String,
    Institution_Phone: String,
    Institution_Web_Address: String
  }, {collection: 'College'});
  mongoose.model('College', schema);
};

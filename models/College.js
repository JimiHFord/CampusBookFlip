var mongoose = require('mongoose');
var model;

try {
  model = mongoose.model('College');
} catch (err) {
  model = mongoose.model('College', {
    Institution_ID: Number,
    Institution_Name: String,
    Institution_Address: String,
    Institution_City: String,
    Institution_State: String,
    Institution_Zip: String,
    Institution_Phone: String,
    Institution_Web_Address: String
  });
}

module.exports = model;

// load config
var _ = require('lodash');
var yaml_config = require('node-yaml-config');
var config = yaml_config.load(__dirname + '/../config/db.yaml');
var dbConfig = config.database;
var mongoose = require('mongoose');

console.log('currently disabled');

// // connect to database
// mongoose.connect('mongodb://'+dbConfig.host+'/'+dbConfig.db);
// // initialize models
// require('../models/initialize')();
// var College = mongoose.model('College');

// College.update({}, {
//   $set: {
//     Institution_Active: true
//   }
// }, {
//   multi: true
// }, function (err, numberAffected, rawResponse) {
//   if(err) { console.log(err); }
//   console.log('updated ' + numberAffected + ' colleges');
//   console.log('raw response', rawResponse);
// });

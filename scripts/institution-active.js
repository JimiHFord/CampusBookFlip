// load config
var yaml_config = require('node-yaml-config');
var config = yaml_config.load('./config/db.yaml');
var dbConfig = config.database;
var mongoose = require('mongoose');
// connect to database
mongoose.connect('mongodb://'+dbConfig.host+'/'+dbConfig.db);
// initialize models
require('../models/initialize')();
var College = mongoose.model('College');

var count = 0;
var printCount = setInterval(function() {
  College.count({}, function(err, total) {
    console.log('Processed ' + count + 'out of ' + total + ' entries...');
  });
}, 5000);

College.find({}, function(err, resultCursor) {
  function processItem(err, item) {
    if(item === null) {
      return;
    }
    processCollege(item, function(err) {
      resultCursor.nextObject(processItem);
    });
  }
  resultCursor.nextObject(processItem);
});

function done() {
  clearInterval(printCount);
}

function processCollege(college, cb) {
  process.nextTick(function() {
    college.Institution_Active = true;
    college.save(function(err) {
      count++;
      cb();
    });
  });
}

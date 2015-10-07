var express = require('express'),
    router = express.Router(),
    mongoose = require('mongoose'),
    College = mongoose.model('College'),
    _ = require('lodash');

// parent module uses "/colleges" for route

function findByCaseInsensitiveName(name, callback) {
  College.find({
    Institution_Name: {
      $regex: '^' + name,
      $options: 'i'
    },
    Institution_Active: true
  }, callback);
};

function findByStateAndCity(state, city, callback) {
  var state = state.toUpperCase();
  College.find({
    Institution_State: state,
    Institution_City: city,
    Institution_Active: true
  }, function(err, colleges) {
    if(err) {
      return callback(err);
    }
    colleges = _.sortBy(colleges, function(obj) {
      return obj.Institution_Name;
    });
    callback(null, colleges);
  });
}

exports.findByStateAndCity = findByStateAndCity;

function stripCollegeAttributes(colleges) {
  return _.map(colleges, function(obj) {
    return {
      name: obj.Institution_Name,
      id: obj.Institution_ID,
      web: obj.Institution_Web_Address
    }
  });
}

exports.stripCollegeAttributes = stripCollegeAttributes;

router.get('/named/:name?', function(req, res) {
  var name = req.param('name', null);
  if(!name) {
    var msg = "\'name\' parameter required";
    return res.json({
      message: msg
    });
  }
  findByCaseInsensitiveName(name, function(err, colleges) {
    if(err) {
      throw err;
    }
    var result = stripCollegeAttributes(colleges);
    res.json(result);
  });
});

router.get('/:state/:city', function(req, res) {
  var state = req.param('state'),
      city  = req.param('city');
  findByStateAndCity(state, city, function(err, colleges) {
    if(err) {
      return res.json(err);
    }
    var result = stripCollegeAttributes(colleges);
    res.json(result);
  });
})

exports.router = router;

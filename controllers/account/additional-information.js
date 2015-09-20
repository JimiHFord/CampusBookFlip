var express = require('express'),
    states = require('../../static-data/states'),
    geography = require('../api/geography'),
    collegesController = require('../api/colleges'),
    mongoose = require('mongoose'),
    Users = mongoose.model('User'),
    router = express.Router();

function stripUser(user) {
  if(user) {
    delete user._id;
    delete user.password;
    return user;
  }
  return {};
}

router.route('/').get(function(req, res) {
  console.log('USER:', req.user);
  var user = stripUser(req.user || {});
  // geography.citiesInState(defaultState, function(err, cities) {
  //   collegesController.findByStateAndCity(defaultState, defaultCity, function(err, colleges) {
  //     colleges = collegesController.stripCollegeAttributes(colleges);
  //
  //   });
  // });
  res.render('account/additional-information', {
    user: user,
    states: states,
    // cities: cities,
    // colleges: colleges
  });
}).post(function(req, res) {
  console.log(req.body);
  Users.findById(req.user._id, function(err, user) {
    if(err) { throw err; }
    var collegeId = parseInt(req.body.college);
    if(!(collegeId in user.colleges)) {
      user.colleges.push(collegeId);
    }
    user.firstName = req.body.firstName;
    user.lastName = req.body.lastName;
    if(user.email !== req.body.email) {
      console.log('Tried to change email');
    }
    user.save(function(err) {
      if(err) { throw err; }
      user = stripUser(user || {});

      res.redirect('/home');
    });


  });
});

module.exports = router;

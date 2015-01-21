var express = require('express'),
    states = require('../../static-data/states'),
    geography = require('../api/geography'),
    collegesController = require('../api/colleges'),
    router = express.Router();

function prepUser(user) {
  return user ? {
    firstName: user.firstName,
    lastName: user.lastName,
    username: user.username,
    email: user.email
  } : {};
}

router.route('/').get(function(req, res) {
  console.log('USER:', req.user);
  var user = prepUser(req.user || {});
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
  var user = prepUser(req.user || {});
  res.render('account/additional-information', {
    user: user,
    states: states
  });
});

module.exports = router;

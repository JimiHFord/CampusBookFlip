var express = require('express'),
    router = express.Router(),
    oauth = require('../config/oauth');


exports.ensureAuthenticated = function (req, res, next) {
  if(req.isAuthenticated()) {
    console.log('authenticated');
    return next();
  }
  console.log('NOT authenticated');
  res.redirect('/');
};

exports.ensureUnauthenticated = function (req, res, next) {
  if(req.isAuthenticated()) {
    return res.redirect('/home');
  }
  next();
}

exports.methods = function(passport) {

  router.get('/facebook', passport.authenticate('facebook', {
    scope: oauth.facebook.loginScope
  }),
    function(req, res) {

    }
  );

  router.get('/facebook/callback', passport.authenticate('facebook', {
    failureRedirect: '/' }),
    function(req, res) {
      // We have to collect additional information
      res.redirect('/account/additional-information');
    }
  );

  router.get('/twitter', passport.authenticate('twitter'), function(req, res) {

  });

  router.get('/twitter/callback', passport.authenticate('twitter', {
    failureRedirect: '/' }), function(req,res) {
      res.redirect('/account/additional-information');
    }
  );

  return router;
};

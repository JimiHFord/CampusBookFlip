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

  // router.get('/facebook', passport.authenticate('facebook', {
  //   scope: oauth.facebook.loginScope
  // }),
  //   function(req, res) {
  //
  //   }
  // );

  router.get('/facebook', function(req, res, next) {
    if(!req.user) {
      passport.authenticate('facebook', {
        scope: oauth.facebook.loginScope
      })(req, res, next);
    } else {
      passport.authorize('facebook', {
        scope: oauth.facebook.loginScope
      })(req, res, next);
    }
  });

  router.get('/facebook/callback', passport.authenticate('facebook', {
    failureRedirect: '/',
    successRedirect: '/account/additional-information'
  }));

  // router.get('/twitter', passport.authenticate('twitter'), function(req, res) {
  //
  // });

  router.get('/twitter', function(req, res, next) {
    if(!req.user) {
      passport.authenticate('twitter')(req, res, next);
    } else {
      passport.authorize('twitter')(req, res, next);
    }
  });

  router.get('/twitter/callback', passport.authenticate('twitter', {
    failureRedirect: '/',
    successRedirect: '/account/additional-information'
  }));

  return router;
};

var express = require('express'),
    router = express.Router();

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

  router.get('/facebook', passport.authenticate('facebook'),
    function(req, res) {

    }
  );

  router.get('/facebook/callback', passport.authenticate('facebook', {
    failureRedirect: '/' }),
    function(req, res) {
      console.log('redirecting to /account');
      res.redirect('/account');
    }
  );

  return router;
};

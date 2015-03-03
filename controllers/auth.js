var express = require('express'),
    router = express.Router();
module.exports = {
  ensureAuthenticated: function (req, res, next) {
    if(req.isAuthenticated()) {
      return next();
    }
    res.redirect('/');
  },
  methods: function(passport) {

    router.get('/facebook', passport.authenticate('facebook'),
    function(req, res) {

      }
    );

    router.get('/facebook/callback', passport.authenticate('facebook', {
      failureRedirect: '/' }),
      function(req, res) {
        res.redirect('/account');
      }
    );

    return router;
  }

};

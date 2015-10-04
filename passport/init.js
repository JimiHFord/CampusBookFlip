var login = require('./login');
var signup = require('./signup');
var mongoose = require('mongoose');
var User = mongoose.model('User');
var FacebookStrategy = require('passport-facebook').Strategy;
var TwitterStrategy = require('passport-twitter').Strategy;
var GithubStrategy = require('passport-github').Strategy;
var GoogleStrategy = require('passport-google').Strategy;
var oauth = require('../config/oauth');

module.exports = function(passport){

	// Passport needs to be able to serialize and deserialize users to support persistent login sessions

    passport.serializeUser(function(user, done) {
      done(null, user._id);
    });

    passport.deserializeUser(function(id, done) {
      User.findById(id, function(err, user) {
        done(err, user);
      });
    });

    // Add more Strategies here
    passport.use(new FacebookStrategy({
      clientID: oauth.facebook.clientID,
      clientSecret: oauth.facebook.clientSecret,
      callbackURL: oauth.facebook.callbackURL,
      passReqToCallback: true // this populates the "req" below
    }, function(req, accessToken, refreshToken, profile, done) {

      // console.log('accessToken',accessToken);
      // console.log('refreshToken',refreshToken);
      console.log('user:',req.user);
      console.log('account:',req.account);
      console.log('req authenticated? ', req.isAuthenticated());
      if(!req.user) {
        // create or login
        User.findOne({
          "facebook.id": profile.id
        }, function(err, user) {
          if(err) { throw err; }
          if(user) {
            done(null, user);
          } else {
            var user = new User({
              facebook: {
                id: profile.id,
                token: accessToken
              },
              username: profile.username,
              firstName: profile._json.first_name,
              lastName: profile._json.last_name,
              email: profile._json.email,
              // We have to register colleges before we allow other permissions
              needsColleges: true
            });

            user.save(function(err) {
              if(err) {
                console.log(err);
              } else {
                done(null, user);
              }
            });
          }
        });
      } else {
        // link existing profile
        var user = req.user;
        user.facebook = {
          id: pofile.id,
          token: accessToken
        };
        user.save(function(err) {
          if(err) { throw err; }
          done(null, req.user); //http://passportjs.org/guide/authorize/
        });
      }

    }));

    passport.use(new TwitterStrategy({
      consumerKey: oauth.twitter.consumerKey,
      consumerSecret: oauth.twitter.consumerSecret,
      callbackURL: oauth.twitter.callbackURL,
      passReqToCallback: true // this populates the "req" below
    }, function(req, token, tokenSecret, profile, done) {
      // console.log('req',req);
      // console.log('token',token);
      // console.log('tokenSecret',tokenSecret);
      console.log('user:',req.user);
      console.log('account:',req.account);
      console.log('req authenticated? ', req.isAuthenticated());
      if(!req.user) {
        // create or login
        User.findOne({
          "twitter.id": profile.id
        }, function(err, user) {
          if(err) { console.log(err); }
          if(!err && user) {
            done(null, user);
          } else {
            var user = new User({
              twitter: {
                id: profile.id,
                token: token
              },
              username: profile.username,
              needsColleges: true
            });
            user.save(function(err) {
              if(err) { throw err; }
              done(null, user);
            })
          }
        });
      } else {
        // link existing
        var user = req.user;
        user.twitter = {
          id: pofile.id,
          token: token
        };
        user.save(function(err) {
          if(err) { throw err; }
          done(null, req.user);
        });
      }
    }));


    // Setting up Passport Strategies for Login and SignUp/Registration
    login(passport);
    signup(passport);

}

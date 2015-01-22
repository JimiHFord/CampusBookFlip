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
    // passport.serializeUser(function(user, done) {
    //     var serialized = {
    //       _id: user._id,
    //       oauthProviders: user.oauthProviders,
    //       username: user.username
    //     };
    //     // console.log('serializing user: ', user);
    //     // done(null, user._id);
    //     done(null, serialized);
    // });
    // TODO: both serialize and deserialize are broken
    // passport.deserializeUser(function(serialized, done) {
    //
    //     console.log(serialized);
    //     if(serialized.username) {
    //       User.findOne({
    //           username: serialized.username
    //         }, function(err, user) {
    //           // console.log('deserializing user:',user);
    //           if(!err) {
    //             done(null, user);
    //           } else {
    //             done(err, null);
    //           }
    //       });
    //     } else {
    //       User.findOne({
    //         oauthProviders: user.oauthProviders
    //       }, function(err, user) {
    //         if(!err) {
    //           done(null, user);
    //         } else {
    //           done(err, null);
    //         }
    //       });
    //     }
    // });

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
      callbackURL: oauth.facebook.callbackURL
    }, function(accessToken, refreshToken, profile, done) {
      // process.nextTick(function() {
      //   return done(null, profile);
      // })
      // console.log(profile);
      User.findOne({
          oauthProviders: {
            oauthID: profile.id
          }
        }, function(err, user) {
        if(err) { console.log(err); }
        if(!err && user) {
          done(null, user);
        } else {
          var user = new User({
            oauthProviders: [{
              provider: oauth.facebook.providerName,
              oauthID: profile.id
            }],
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
    }));

    passport.use(new TwitterStrategy({
      consumerKey: oauth.twitter.consumerKey,
      consumerSecret: oauth.twitter.consumerSecret,
      callbackURL: oauth.twitter.callbackURL
    }, function(token, tokenSecret, profile, done) {
      // process.nextTick(function() {
      //   return done(null, profile);
      // })
      console.log(profile);
      // User.findOne({
      //   oauthProviders: {
      //     oauthID: profile.id
      //   }
      // }, function(err, user) {
      //   if(err) { console.log(err); }
      //   if(!err && user) {
      //     done(null, user);
      //   } else {
      //     var user = new User({
      //       oauthProviders: [{
      //         provider: oauth.facebook.providerName,
      //         oauthID: profile.id
      //       }],
      //       username: profile.username,
      //       firstName: profile._json.first_name,
      //       lastName: profile._json.last_name,
      //       email: profile._json.email,
      //       // We have to register colleges before we allow other permissions
      //       needsColleges: true
      //     });
      //
      //     user.save(function(err) {
      //       if(err) {
      //         console.log(err);
      //       } else {
      //         done(null, user);
      //       }
      //     });
      //   }
      // });
      done(profile);
    }));


    // Setting up Passport Strategies for Login and SignUp/Registration
    login(passport);
    signup(passport);

}

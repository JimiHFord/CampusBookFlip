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

        console.log('serializing user: ', user);
        done(null, user._id);
    });

    passport.deserializeUser(function(id, done) {
        User.findById(id, function(err, user) {
            console.log('deserializing user:',user);
            if(!err) {
              done(null, user);
            } else {
              done(err, null);
            }
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
      console.log(profile);
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
              provider: 'Facebook',
              oauthID: profile.id
            }],
            name: profile.displayName,
            needsColleges: true
          });
          // We have to register colleges before we save the user
          done(null, user);
          // user.save(function(err) {
          //   if(err) {
          //     console.log(err);
          //   } else {
          //     console.log('saving user...');
          //     done(null, user);
          //   }
          // });
        }
      });
    }));

    // Setting up Passport Strategies for Login and SignUp/Registration
    login(passport);
    signup(passport);

}

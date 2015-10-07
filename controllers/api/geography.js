var express = require('express'),
    router = express.Router(),
    mongoose = require('mongoose'),
    College = mongoose.model('College'),
    _ = require('lodash'),
    async = require('async'),
    states = require('../../static-data/states');

function citiesInState(state, myCallback) {
  var state = state.toUpperCase();
  College.find({
    Institution_State: state,
    Institution_Active: true
  }, function(err, colleges) {
    var cities = [];
    var helper = {}
    async.each(colleges, function(obj, callback) {
      var city = obj.Institution_City;
      if(!(city in helper)) {
        helper[city] = true;
        cities.push(city);
      }
      // cities[city].push(city);
      callback();
    }, function(err) {
      if(err) {
        myCallback(err);
      }
      myCallback(null, _.sortBy(cities, function(city) {
        return city;
      }));
    });
  });
}

exports.citiesInState = citiesInState;

router.get('/states', function(req, res) {
  res.json(states);
});

router.get('/state/:state/cities', function(req, res) {
  var state = req.param('state');
  citiesInState(state, function(err, cities) {
    if(err) {
      return res.json(err);
    }
    return res.json(cities);
  });
});

// router.get('/start', function(req, res) {
//   var count = 8575;
//   College.find({}, function(err, colleges) {
//     if(err) {
//       return res.json(err);
//     }
//     async.each(colleges, function(obj, callback) {
//       obj.Institution_Zip = obj.Institution_Zip.replace(/['"]+/g, '');
//       obj.save(function(err, fixed) {
//         if(err) {
//           console.log(err);
//         }
//         count -= 1;
//         console.log(count + ' left, fixed: ' + fixed.Institution_Zip);
//         callback();
//       });
//     }, function(err) {
//       console.log('FINISHED!!!')
//     });
//     res.json({
//       working: true
//     });
//   });
// });

// router.get('/start', function(req, res) {
//   var count = 8575;
//   College.find({}, function(err, colleges) {
//     if(err) {
//       return res.json(err);
//     }
//     async.each(colleges, function(obj, callback) {
//       if(!obj.Institution_Web_Address) {
//         obj.Institution_Active = false;
//         obj.save(function(err, fixed) {
//           if(err) {
//             console.log(err);
//           }
//           count -= 1;
//           console.log(count + ' left, deactivated: ' + fixed.Institution_Name);
//
//         });
//       } else {
//         obj.Institution_Active = true;
//         obj.save(function(err, fixed) {
//           if(err) {
//             console.log(err);
//           }
//           count -= 1;
//           console.log(count + ' left');
//         });
//       }
//       callback();
//     }, function(err) {
//       console.log('FINISHED!!!')
//     });
//     res.json({
//       working: true
//     });
//   });
// });

exports.router = router;

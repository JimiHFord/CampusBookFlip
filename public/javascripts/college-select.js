(function($) {
  var state = $('.select-state'),
      city  = $('.select-city'),
      college = $('.select-college'),
      cityLoading = $('.select-city-loading'),
      collegeLoading = $('.select-college-loading'),
      cache = {};

  var citiesEndpoint = {
    h1: '/api/geography/state/',
    h2: '/cities'
  };

  var collegesEndpoint = {
    h1: '/api/colleges/', // state
    h2: '/' // city
  };

  // var defaultState = state.val();

  // populate default city and default colleges
  // getCities(defaultState, function(err, cities) {
  //   var defaultCity;
  //   for(var cty in cities) {
  //     defaultCity = cty;
  //     break;
  //   }
  //   getColleges(defaultState, defaultCity, function(err, colleges) {
  //
  //     college.html(htmlForColleges(colleges));
  //   });
  // });


  state.on('change', function(ev) {
    var selectedState = state.val();
    college.hide();
    city.hide();
    cityLoading.show();
    getCities(selectedState, function(err, data) {
      if(err) {
        return console.log(err);
      }
      city.html(htmlForCities(data));
      cityLoading.hide();
      city.show();
    });
  });

  city.on('change', function(ev) {
    var selectedCity = city.val(),
    selectedState = state.val();
    college.hide();
    collegeLoading.show();
    getColleges(selectedState, selectedCity, function(err, colleges) {
      if(err) {
        return console.log(err);
      }
      college.html(htmlForColleges(colleges));
      collegeLoading.hide();
      college.show();
    });
  });

  college.on('change', function(ev) {

  });

  function htmlForColleges(colleges) {
    var html = '<option value="">Select college...</option>';
    if(colleges) {
      for(var i = 0; i < colleges.length; i++) {
        var obj = colleges[i];
        html += '<option value="'+obj.id+'">'+obj.name+'</option>';
      }
    }
    return html;
  }

  function htmlForCities(cities) {
    var html = '<option value="">Select city...</option>';
    if(cities) {
      for(var obj in cities) {
        // var obj = cities[i];
        html += '<option value="'+obj+'">'+obj+'</option>';
      }
    }
    return html;
  }

  function getColleges(state, city, callback) {
    if(!(cache[state][city] && cache[state][city].length)) {
      $.ajax({
        method: 'GET',
        url: collegesEndpoint.h1 + state + collegesEndpoint.h2 + city,
        success: function(data) {
          cache[state][city] = data;
          callback(null, cache[state][city]);
        },
        error: function(data) {
          callback(data);
        }
      });
    } else {
      callback(null, cache[state][city]);
    }
    // if(!(cache[state])) {
    //   getCities(state, function(err, data) {
    //
    //   });
    // }
  }

  function getCities(state, callback) {
    if(!cache[state]) {
      cache[state] = {};
      $.ajax({
        method: 'GET',
        url: citiesEndpoint.h1 + state + citiesEndpoint.h2,
        success: function(data) {
          for(var i = 0; i < data.length; i++) {
            var cty = data[i];
            cache[state][cty] = [];
          }
          callback(null, cache[state]);
        },
        error: function(data) {
          callback(data);
      }});
    } else {
      callback(null, cache[state]);
    }
  }
}(jQuery));

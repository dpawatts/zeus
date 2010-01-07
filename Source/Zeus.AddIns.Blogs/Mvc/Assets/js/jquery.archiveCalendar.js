(function($)
{
	$.fn.archiveCalendar = function(url)
	{
		// create a calendar for each match
		this.each(function()
		{
			new $.archiveCalendar(this, url);
		});

		return this;
	};

	$.archiveCalendar = function(el, url)
	{
		function monthName(month)
		{
			switch (month)
			{
				case 0:
					return "January";
				case 1:
					return "February";
				case 2:
					return "March";
				case 3:
					return "April";
				case 4:
					return "May";
				case 5:
					return "June";
				case 6:
					return "July";
				case 7:
					return "August";
				case 8:
					return "September";
				case 9:
					return "October";
				case 10:
					return "November";
				case 11:
					return "December";
				default:
					return "unknown";
			}
		}

		// determin number of days in a month        
		function daysInMonth(iMonth, iYear)
		{
			return 32 - new Date(iYear, iMonth, 32).getDate();
		}

		// change the date object day from a Sunday start to a Monday Start!
		function dayToCalDay(iday)
		{
			if (iday == 0)
			{
				calDay = 6;
			} else
			{
				calDay = iday - 1;
			}
			return calDay;
		}

		function onDay(theDay)
		{
			$('#calTable tr').each(function()
			{
				$(this).children('td:eq(' + theDay + ')').addClass('on');
			});
		}

		// populate calendar based on the year / month and populate with posts and links via ajax request
		function popCal(intMonth, intYear)
		{
			newDate = new Date();
			newDate.setFullYear(intYear);
			newDate.setMonth(intMonth);
			newDate.setDate(1);
			startDay = dayToCalDay(newDate.getDay());
			iDay = 1;
			lastDay = daysInMonth(intMonth, intYear);

			$('#calTable').fadeTo(50, 0);

			$('#calTable td').removeClass('selected').removeAttr('dayRef');
			$("#calTable td").each(function(i)
			{
				$(this).html('').addClass('none');
				//if its not going to be an empty cell...
				if (iDay <= lastDay && i >= startDay)
				{
					$(this).html(iDay).attr('dayRef', iDay);
					iDay++;
				}
			});

			$.getJSON(url, { month: intMonth, year: intYear }, function(json)
			{
				jsonPosts = json;
				$.each(jsonPosts.dates, function(i)
				{
					$("#calTable td[dayRef='" + jsonPosts.dates[i].date + "']").wrapInner('<a href="' + jsonPosts.dates[i].url + '"></a>').addClass('selected').removeClass('none');
					//alert('woo');
				});
				$('#calTable').fadeTo(50, 1);
				bindCalEvents();
			});


			$('#calInner div span').html(monthName(intMonth) + " " + intYear).attr("monthInt", intMonth).attr("yearInt", intYear);
		}

		// hovers for the cells + click to update date
		function bindCalEvents()
		{
			$('#calTable td').unbind();

			theCells = $('#calTable td').not('.none');


			theCells.hover(function()
			{
				$(this).addClass('hover');
			}, function()
			{
				$(this).removeClass('hover');

			}).click(function()
			{
				window.location = $(this).children('a').attr('href');
			});
		}

		/// First load and event handlers ///


		var d = new Date();
		thisMonth = d.getMonth();
		thisYear = d.getFullYear();
		popCal(thisMonth, thisYear);
		navDate = d;

		noBack = true;

		$('#calForward').addClass('hover').click(function()
		{
			navDate.setMonth(navDate.getMonth() + 1);
			popCal(navDate.getMonth(), navDate.getFullYear());
			if (noBack = true)
			{
				noBack = false;
				$('#calBack').removeClass('stop');
			}
			return false;
		});

		$('#calBack').click(function()
		{

			navDate.setMonth(navDate.getMonth() - 1);
			popCal(navDate.getMonth(), navDate.getFullYear());
			// if(navDate.getMonth() == thisMonth && navDate.getFullYear() == thisYear) {
			//                 noBack = true;
			//                 $('#calBack').addClass('stop');
			//             }
			return false;

		});
	};
})(jQuery);
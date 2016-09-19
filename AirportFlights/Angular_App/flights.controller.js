(function () {
    'use strict';

    angular.module('flightApp').controller('FlightListController', FlightListController);

    function FlightListController($http) {
        var vm = this;
        var dataService = $http;
        vm.flights = [];
        vm.flight = {};
        vm.gates = [];
        vm.selectedGate = { GateName: '', GateNumber: '' };
        vm.selectorGate = { GateName: '', GateNumber: '' };
        // Hook up public events
        //vm.assignClick = assignClick;
        vm.cancelClick = cancelClick;
        vm.editClick = editClick;
        vm.deleteClick = deleteClick;
        vm.saveClick = saveClick;

        var pageMode = {
            LIST: 'List',
            EDIT: 'Edit',
            ADD: 'Add'
        };
        vm.uiState = {
            mode: pageMode.LIST,
            isDetailAreaVisible: false,
            isListAreaVisible: true,
            isValid: true,
            messages: []
        };
        gateList();

        var eventsData = [];
        flightCalendarUI();

        function gateList() {
            dataService.get("/api/Gate/AllGates")
              .then(function (result) {
                  vm.gates= result.data;

              }, function (error) {
                  handleException(error);
              });
        }

        function flightList(gateNumber) {
            var urlGate = "/api/FlightApi/AllFlights";
            if ((typeof gateNumber != "undefined") && !(gateNumber == null))
                urlGate = urlGate + "/" + gateNumber;
            else
                urlGate = urlGate + "/0";
            dataService.get(urlGate)
            .then(function (result) {
                vm.flights = result.data;
                setUIState(pageMode.LIST);
            },
            function (error) {
                handleException(error);
            });
        }

        function handleException(error) {
            alert(error.data.ExceptionMessage);
        }
        function editClick(id) {
            flightGet(id);
            setUIState(pageMode.EDIT);
        }
        function cancelClick(flightForm) {
            flightForm.$setPristine();
            flightForm.$valid = true;
            vm.uiState.isValid = true;

            setUIState(pageMode.LIST);
        }
        function saveClick() {  
            updateData();
        }

        function deleteClick(id) {
            if (confirm("Are you sure you want to cancel this flight?")) {
                deleteData(id);
            }
        }
        function deleteData(id) {
            var selectedGate = $('#gateSelector').val();
            dataService.delete(
                        "/api/FlightApi/DeleteFlight/" + id)
                .then(function (result) {
                    showUpdatedEvents(selectedGate);
                    setUIState(pageMode.LIST);
                }, function (error) {
                    handleException(error);
                });
        }
        function flightGet(id) {
            dataService.get("/api/FlightApi/GetFlight/" + id)
              .then(function (result) {
                  vm.flight = result.data;
                  vm.selectedGate.GateNumber = vm.flight.GateNumber;

              }, function (error) {
                  handleException(error);
              });
        }

        function updateData() {
            dataService.put("/api/FlightApi/UpdateFlight" 
                  ,vm.flight)
              .then(function (result) {
                  // Update object
                  vm.flight = result.data;
                  showUpdatedEvents(vm.flight.GateNumber);
                  setUIState(pageMode.LIST);
              }, function (error) {
                  handleException(error);
              });
        }

        function setUIState(state) {
            vm.uiState.mode = state;

            vm.uiState.isDetailAreaVisible =
              (state == pageMode.ADD || state == pageMode.EDIT);
            vm.uiState.isListAreaVisible = (state == pageMode.LIST);
        }
        function flightCalendarUI() {

            var d = new Date();
            var month = d.getMonth() + 1;
            var day = d.getDate();
            var output = d.getFullYear() + '-' +
                (month < 10 ? '0' : '') + month + '-' +
                (day < 10 ? '0' : '') + day;

            $('#calendar').fullCalendar({
                schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
                header: {
                    left: '',
                    center: 'title',
                    right: ''
                },
                defaultDate: output,
                nowIndicator: true,
                aspectRatio: 5,
                scrollTime: '00:00',
                allDaySlot: false,
                lazyFetching: false,
                //editable: true,
                //selectable: true,
                defaultView: 'timelineDay',
                //defaultView: 'basicDay',
                timezone: 'local',
                views: {
                    timelineDay: { slotDuration: '00:30' }
                },
                events: eventsData,
                resources: [
                        { id: 'Gate1', title: 'Gate A', eventColor: 'green' }
                        , { id: 'Gate2', title: 'Gate B', eventColor: 'blue' }
                ],
                resourceAreaWidth: '15%',
                resourceLabelText: 'Gate Name',
                eventClick: function (calEvent, jsEvent, view) {

                    vm.editClick(calEvent.title)
                    $(this).css('border-color', 'red');
                }
            });

        }

        function showUpdatedEvents(selectedGate) {

            var urlJson = "Flight/GetJsonData/" + selectedGate;
            $.getJSON(urlJson, null, function (data) {

                $('#calendar').fullCalendar('removeEventSource', eventsData);
                eventsData = data;

            }).then(
              function () {
                  $('#calendar').fullCalendar('addEventSource', eventsData);
                  flightList(selectedGate);
                  //alert(JSON.stringify(eventsData));
                  //$('#calendar').fullCalendar('refetchEventSources', eventsData);

              }, function () {
                  alert("Sorry we were not able to fetch flight data!");
              });
        }

        $('#btnGetData').click(function () {
            var selectedGate = $('#gateSelector').val();
            showUpdatedEvents(selectedGate);
            //$("#gateSelector" ).change(function() {
        });

    }
    angular.module('flightApp').controller('FlightAddController', FlightAddController);

    function FlightAddController($http) {
        $scope.message = 'Please add a new flight here';
    }
})();
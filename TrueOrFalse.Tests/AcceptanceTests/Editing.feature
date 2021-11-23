Feature: Editing
	In order to provide the ability to play in a "TrueOrFalse" game
	As an Editor
	I want to keep track of game statements

	Background:
		Given I have five statements

Scenario: Adding a statement
	When I add one statement		
	Then it gets saved and I can get back to it

Scenario: Editing and Saving a Statement
	Given I added one statement
		And Current Statement is Not Empty
	When I Edit both text and statement's flag
		And Save the Editings
	Then It gets changed

Scenario: Remove a statement
	Given I added two statements
	When I remove one of them
	Then Only one statement remains in the list

Scenario: Cut the statement text
	Given I added one statement
		And Current Statement is Not Empty
	When I cut the statement's text
	Then It gets removed from the UI and saved into buffer

	

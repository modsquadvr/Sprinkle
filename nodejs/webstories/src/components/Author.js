// author.js

import React, { Component } from "react";
import "../css/author.css";

class Author extends Component {
  render() {
    return (
      <div className="guide container">
        <h2>
          Author’s Guide for <span>VicStories</span>
        </h2>

        <p>&nbsp;</p>

        <p>
          The Ink language is very flexible, and you should feel free to use
          anything in it you like. There is a short authoring guide at{" "}
          <a href="https://www.inklestudios.com/ink/web-tutorial/">
            https://www.inklestudios.com/ink/web-tutorial/
          </a>
          and a much more complete one at{" "}
          <a href="https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md">
            https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md
          </a>
        </p>

        <p>
          There will eventually be a nice editor on this page and you won't have
          to go anywhere else to author a story. We're not quite there, yet,
          though. In the meantime I would suggest downloading the Inky authoring
          tool from here:{" "}
          <a href="https://github.com/inkle/inky">
            https://github.com/inkle/inky
          </a>{" "}
          and then emailing in your stories to be added to the website. We'll
          get the kinks worked out and then make it easier.
        </p>

        <p>
          In order to fit within the confines of our application, though, we
          will need to define some standardized behaviour, <span>and also</span>{" "}
          some rules for how to add images, etc.
        </p>

        <p>
          The first standards are some set variables. The most important being
          location. We will have to have a set list of locations by the time we
          ship, and there will be a little bit of work to add new locations.
          Locations will be found in a global variable named “Place” which will
          take a string. Please send mail to{" "}
          <a href="mailto:vicstories@lists.uvic.ca">vicstories@lists.uvic.ca</a>{" "}
          to have any places added.
        </p>

        <p>Let’s start with the following:</p>

        <p>
          “<span>cityhall</span>” city hall
        </p>

        <p>“interactivity” Interactivity board games café</p>

        <p>
          “<span>victheater</span>” the Vic theater
        </p>

        <p>“cineplex” the Cineplex Odeon</p>

        <p>“capitol6” the capitol 6</p>

        <p>
          “<span>VFFoffice</span>” the VFF office
        </p>

        <p>
          We will certainly add some additional locations, but this set will
          allow the testing of stories within a close area to make initial
          authoring easier.
        </p>

        <p>
          The next variables are to do with some general preferences. We want
          users to be able to decide how to interact with the stories globally
          rather than for each story. Most of these will not apply to people
          playing on the website since we are limited in what we can do there.
        </p>

        <p>
          “Notify” has the options “never”, “app open”, and “always” which
          determines whether the user wants a notification when new story
          content becomes available. For instance, if story content triggers on
          proximity to city hall, will just walking pass cause an alert? Or will
          you need to have the app in the foreground to see that something has
          happened.
        </p>

        <p>
          “Location” has the options “auto” and “manual”. This is to allow
          players on mobile devices to set their location via a checkbox if they
          want to play the whole game from one location. This is a variable
          because as a story author you are welcome to hide things that are ONLY
          visible from a place, but in general you should make everything
          available to lazy players.
        </p>

        <p>
          “Sound” has the options “sound” and “silent”. It is not clear how much
          use of sound stories will provide, but this is to allow for the
          option.
        </p>

        <p>
          Each image or sound in a story will have to be uploaded with the
          story. In order to make organization of this easier, there will be a
          form on the author page asking for several pieces of information. The
          first is the story name, this should be a one-word unique name to
          identify the story. This will not be shown to players. For instance,
          if I am writing a story about a dog and a <span>boy</span> I might
          choose <span>dogandboy</span>
          as my unique name. This name is to be used for all submission for this
          story. The mandatory pieces are a text file that contains the summary
          information for the story:
        </p>

        <p>Title – the full title of the story, as seen by the user</p>

        <p>
          Summary – a short summary of the story. Something to get the viewer
          hooked.
        </p>

        <p>
          Movie – the movie that inspired the story (NB this field will be
          hidden on the website until after the Jan.18 movie announcements)
        </p>

        <p>Author – the name (or names) of the author of the story</p>

        <p>
          The story itself will be in an “ink” file. This is a text file that
          contains the source for the story.
        </p>

        <p>
          The story must also be delivered in a compiled form as a json file.
        </p>

        <p>
          Finally, any images or sound files should be numbered sequentially –
          dogandboy1.jpg, dogandboy2.jpg, dogandboy1.mp3, etc.
        </p>

        <p>
          When the authoring portion of the webpage is finished there will be a
          form for the summary, and editor for the story, and an upload for the
          images and sounds that will make these naming decisions automatic.
        </p>

        <p>&nbsp;</p>
      </div>
    );
  }
}

export default Author;

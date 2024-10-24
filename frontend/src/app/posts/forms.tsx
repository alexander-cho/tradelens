import { useState } from "react";

const POSTS_URL = "/api/posts/";

const PostForm = () => {
  const [formIsFetched, setFormIsFetched] = useState(false);

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    console.log(event, event.currentTarget);

    const formData = new FormData(event.currentTarget);

    // list of form input key-value pairs into object
    const formAsObject = Object.fromEntries(formData);

    const formAsJson = JSON.stringify(formAsObject);

    const requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: formAsJson,
    };

    const response = await fetch(POSTS_URL, requestOptions);
    const data = response.json();
    console.log(data);
  }

  function handleShowPostFormClick() {
    setFormIsFetched(true);
  }

  function handleHidePostFormClick() {
    setFormIsFetched(false);
  }

  if (!formIsFetched) {
    return (
      <button onClick={handleShowPostFormClick}>
        Click here to share an idea.
      </button>
    );
  } else {
    return (
      <>
        Share an idea.
        <form onSubmit={handleSubmit}>
          <div>
            <label htmlFor="ticker">Ticker</label>
            <input type="ticker" name="ticker" id="ticker" required />
          </div>
          <div>
            <label htmlFor="body">Message</label>
            <input type="body" name="body" id="body" required />
          </div>
          <div>
            <label htmlFor="sentiment">Sentiment</label>
            <input type="sentiment" name="sentiment" id="sentiment" required />
          </div>
          <button type="submit">Submit</button>
        </form>
        <button onClick={handleHidePostFormClick}>
          Click here to revert.
        </button>
      </>
    );
  }
};

export default PostForm;

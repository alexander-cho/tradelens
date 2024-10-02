import { useState } from "react";

const POSTS_URL = "/api/posts/";

const PostForm = () => {
//   const [formData, setFormData] = useState({
//     ticker: "",
//     body: "",
//     sentiment: "",
//   });
//   async function handleSubmit(event: Event) {
//     event.preventDefault();
//     console.log(event, event.target);

//     const requestOptions = {
//       method: "GET",
//       headers: {
//         "Content-Type": "application/json",
//       },
//       body: JSON.stringify(formData),
//     };

//     // turn submitted raw form data into json.
//     const response = await fetch(POSTS_URL, requestOptions);
//     const data = response.json();
//     console.log(data);
//   }

  return (
    <>
      <div>Share an idea.</div>
      {/* <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="username">Username</label>
          <input
            type="text"
            name="username"
            id="username"
            value={formData.ticker}
          />
        </div>
        <div>
          <label htmlFor="password">Password</label>
          <input
            type="password"
            name="password"
            id="password"
            value={formData.body}
          />
        </div>
        <div>
          <label htmlFor="password">Password</label>
          <input
            type="password"
            name="password"
            id="password"
            value={formData.sentiment}
          />
        </div>
        <button type="submit">Submit</button>
      </form> */}
    </>
  );
};

export default PostForm;

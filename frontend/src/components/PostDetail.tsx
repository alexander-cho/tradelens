import React, { useState } from "react";
const POST_URL = "/api/posts/[id]/";

interface Post {
  ticker: string;
  body: string;
  sentiment: string;
}

const Post = () => {
  const [isDataFetched, setIsDataFetched] = useState(false);
  const [post, setPost] = useState<Post | undefined>();

  async function getOnePost() {
    const res = await fetch(POST_URL);
    const post = await res.json();
    // console.log(post);
    setPost(post);
  }

  function handleShowPostClick() {
    setIsDataFetched(true);
    getOnePost();
  }

  function handleHidePostClick() {
    setIsDataFetched(false);
  }

  if (!isDataFetched) {
    return (
      <>
        <button onClick={handleShowPostClick}>
          Click here to get one post.
        </button>
      </>
    );
  } else {
    return (
      <>
        <div>
            {JSON.stringify(post)}
        </div>
        <button onClick={handleHidePostClick}>Click here to go back.</button>
      </>
    );
  }
};

export default Post;

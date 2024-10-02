"use client";

import React, { useState } from "react";
import PostForm from "./forms";

const POSTS_URL = "/api/posts/";

const AllPosts = () => {
  const [isDataFetched, setIsDataFetched] = useState(false);
  const [posts, setPosts] = useState<any[]>([]);

  async function getAllPosts() {
    try {
      const res = await fetch(POSTS_URL);
      const posts = await res.json();
      // console.log(posts);
      setPosts(posts);
    } catch (err) {
      console.error(err);
    }
  }

  function handleShowPostsClick() {
    setIsDataFetched(!isDataFetched);
    getAllPosts();
  }

  if (!isDataFetched) {
    return (
      <>
        <button onClick={handleShowPostsClick}>
          Click here to retrieve all the posts.
        </button>
        <PostForm />
      </>
    );
  }

  return (
    <>
      <div>{JSON.stringify(posts)}</div>
      <button>Click here to revert.</button>
    </>
  );
};

export default AllPosts;

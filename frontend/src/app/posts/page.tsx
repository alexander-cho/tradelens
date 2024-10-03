"use client";

import React, { useState } from "react";
import PostForm from "./forms";
import GetOnePost from "./[id]/page";

const POSTS_URL = "/api/posts/";
const POST_URL = "/api/posts/[id]/";

interface Post {
  ticker: string;
  body: string;
  sentiment: string;
}

const GetAllPosts = () => {
  const [isDataFetched, setIsDataFetched] = useState(false);
  const [posts, setPosts] = useState<Post | undefined>();

  const [isOnePostFetched, setIsOnePostFetched] = useState(false);
  const [post, setPost] = useState<Post | undefined>();

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

  async function getOnePost() {
    const res = await fetch(POST_URL);
    const post = await res.json();
    console.log(post);
    setPost(post);
  }

  function handleShowPostsClick() {
    setIsDataFetched(true);
    getAllPosts();
  }

  function handleHidePostsClick() {
    setIsDataFetched(false);
  }

  function handleOnePostClick() {
    setIsOnePostFetched(true);
    getOnePost();
  }

  if (!isDataFetched) {
    return (
      <>
        <button onClick={handleShowPostsClick}>
          Click here to retrieve all the posts.
        </button>
        <button onClick={handleOnePostClick}>
          Click here to get one post.
        </button>
        <PostForm />
        <GetOnePost/>
      </>
    );
  } else if (isOnePostFetched) {
    return (
      <>
        <div>{JSON.stringify(post)}</div>
        <button onClick={handleHidePostsClick}>Click here to revert.</button>
      </>
    );
  } else {
    return (
      <>
        <div>{JSON.stringify(posts)}</div>
        <button onClick={handleHidePostsClick}>Click here to revert.</button>
      </>
    );
  }
};

export default GetAllPosts;

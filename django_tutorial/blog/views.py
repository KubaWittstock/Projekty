from django.shortcuts import render, get_object_or_404, get_list_or_404
from django.http import Http404
from .models import Post

# Create your views here.


def home(request):
    latest_post = Post.objects.latest()
    return render(request, "blog/home.html", {"latest_post": latest_post})


def all_posts(request):
    posts = Post.objects.all().order_by("-date_created")
    return render(request, "blog/all_posts.html", {"all_posts": posts})


def post(request, post_slug):
    post = get_object_or_404(Post, slug__exact=post_slug)
    return render(request, "blog/post.html", {"post": post})


def posts_with_tag(request, tag):
    posts_with_tag = get_list_or_404(Post.objects.order_by('-date_created'), tags__slug__exact=tag )
    return render(request, "blog/tags.html", {"post_with_tag": posts_with_tag})

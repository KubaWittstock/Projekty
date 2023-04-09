import datetime
from django.shortcuts import render
from django.http import Http404

# Create your views here.

posts = [
    {
        "slug": "the-ethical-implications-of-artificial-intelligence",
        "title": "The Ethical Implications of Artificial Intelligence",
        "content": "Artificial intelligence (AI) is rapidly transforming society, offering numerous benefits but also raising ethical concerns. In this article, we delve into the pressing ethical issues surrounding AI, from bias and privacy concerns to the potential for malicious use, and explore why addressing these concerns is crucial for the future of AI.",
        "summary": "From bias to malicious use, we explore the ethical concerns around AI and why they matter.",
        "tags": [{"display_name": "Ethics", "slug": "ethics"}, {"display_name": "AI", "slug": "ai"}, {"display_name": "Privacy", "slug": "privacy"}],
        "date_created": datetime.date(2023, 1, 23),
        "img": {"dir": "blog/imgs/the-ethical-implications-of-artificial-intelligence.jpg", "alt": "the-ethical-implications-of-artificial-intelligence"},
    },
    {
        "slug": "how-ai-is-changing-the-job-market",
        "title": "The Future of Work: How AI is Changing the Job Market",
        "content": "Artificial intelligence (AI) is transforming the job market, with some jobs at risk of automation and new job opportunities emerging from AI. In this article, we take a closer look at the impact of AI on the job market, identify which jobs are most at risk and which ones are less likely to be affected, and explore the exciting new job opportunities that are emerging from this technology.",
        "summary": "AI is revolutionizing the job market – find out which jobs are at risk and which ones are thriving.",
        "tags": [{"display_name": "Future of Work", "slug": "future-of-work"}, {"display_name": "Automation", "slug": "automation"}, {"display_name": "AI", "slug": "ai"}],
        "date_created": datetime.date(2023, 1, 15),
        "img": {"dir": "blog/imgs/how-ai-is-changing-the-job-market.jpg", "alt": "how-ai-is-changing-the-job-market"},
    },
    {
        "slug": "the-science-of-artificial-intelligence-explained",
        "title": "The Science of Artificial Intelligence: An Overview",
        "content": "Artificial intelligence (AI) is a fascinating and rapidly evolving field that holds immense promise for the future. In this article, we provide a comprehensive overview of the science behind AI, from supervised and unsupervised learning to neural networks and deep learning, and explore the cutting-edge research and technological breakthroughs that are driving this field forward.",
        "tags": [{"display_name": "AI Science", "slug": "ai-science"}, {"display_name": "Neural Networks", "slug": "neural-networks"}, {"display_name": "AI", "slug": "ai"}],
        "date_created": datetime.date(2023, 2, 10),
        "img": {"dir": "blog/imgs/the-science-of-artificial-intelligence-explained.jpg", "alt": "the-science-of-artificial-intelligence-explained"},
    },
    {
        "slug": "the-amazing-world-of-pi",
        "title": "The Amazing World of Pi: Exploring the Famous Number",
        "content": "Pi is one of the most famous mathematical constants, used in a wide range of scientific and engineering applications. In this article, we explore the fascinating world of pi, from its history and mathematical properties to its practical applications in fields such as computer science, physics, and engineering.",
        "summary": "Discover the fascinating world of pi, from its mathematical properties to its real-world applications.",
        "tags": [{"display_name": "Pi", "slug": "pi"}, {"display_name": "Mathematics", "slug": "mathematics"}, {"display_name": "Applications", "slug": "applications"}],
        "date_created": datetime.date(2023, 3, 14),
        "img": {"dir": "blog/imgs/the-amazing-world-of-pi.jpg", "alt": "the-amazing-world-of-pi"},
    }
]


def home(request):
    sorted_posts = sorted(posts, key=lambda x: x['date_created'])
    latest_post = sorted_posts[-1]

    return render(request, "blog/home.html", {"latest_post": latest_post})


def all_posts(request):
    return render(request, "blog/all_posts.html", {"all_posts": posts})


def post(request, post_slug):
    try:
        post = list(filter(lambda x: x['slug'] == post_slug, posts))[0]
        return render(request, "blog/post.html", {"post": post})
    except Exception as e:

        raise Http404()


def posts_with_tag(request, tag):
    posts_with_tag = [post for post in posts
                      if tag in [tag["slug"] for tag in post["tags"]]]

    if len(posts_with_tag) > 0:
        tag = list(filter(lambda x: x['slug'] == tag, posts_with_tag[0]['tags']))[
            0]['display_name']

    return render(request, "blog/tags.html", {"tag": tag, "post_with_tag": posts_with_tag})

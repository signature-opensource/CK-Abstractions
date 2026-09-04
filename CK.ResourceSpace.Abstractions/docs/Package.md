The contracts that let a type declare which resources it owns and where it sits among the others.

IResourceGroup marks a group of resources; IResourcePackage restricts its children to belong to no other
package, so a child may be in many groups but in at most one package.

Five attribute families declare that placement - Package, Children, Groups, Requires and RequiredBy -
each usable either with package full names as strings or with the types themselves. Four of them have an
optional variant whose relation applies only if the named package is part of the selection, and is
dropped otherwise.
